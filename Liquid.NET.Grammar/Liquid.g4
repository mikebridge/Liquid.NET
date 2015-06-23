// Some inspiration from: https://github.com/bkiers/Liqp/blob/master/src/grammar/Liquid.g

grammar Liquid;
@parser::header {#pragma warning disable 3021}

options {
  tokenVocab=LiquidLexer;
}

// todo: rename outputxperssion to liquidexpression

/*
 * Parser Rules
 */

init:				block*; 

block:				outputmarkup | tag | rawtext ;

// TODO: name this something other than rawtext (e.g. ocean text)
rawtext:			TEXT+ ;

				
	
// {% Parse tags %}

tag:				raw_tag
					| custom_blocktag  //{System.String.Equals("end" + custom_block_start_tag().GetText(), custom_block_end_tag().GetText())}?
					| custom_tag
					| if_tag
					| for_tag
					| cycle_tag
					| assign_tag
					| capture_tag
					| increment_tag
					| decrement_tag
					| unless_tag										
					| case_tag
					// | table_tag
					| include_tag
					| break_tag
					| continue_tag
					| macro_tag
					;

// text wrapped in a raw tag
raw_tag:			RAW;

//raw_tag:			RAW_START .*? RAW_END;


// TODO: Clean up this semantic predicate so it gives a decent error.
//custom_blocktag:	TAGSTART custom_block_start_tag customtagblock_expr* TAGEND custom_blocktag_block TAGSTART custom_block_end_tag { _localctx.custom_block_end_tag().GetText().Equals("end" + _localctx.custom_block_start_tag().GetText()) }?  TAGEND ;
custom_blocktag:	TAGSTART custom_block_start_tag customtagblock_expr* TAGEND custom_blocktag_block TAGSTART custom_block_end_tag TAGEND { _localctx.custom_block_end_tag().GetText().Equals("end" + _localctx.custom_block_start_tag().GetText()) }?;
//					| TAGSTART custom_block_start_tag customtagblock_expr* TAGEND custom_blocktag_block TAGSTART LABEL TAGEND {NotifyErrorListeners("Liquid error: end tag does not match start tag '" + _localctx.custom_block_start_tag().GetText() + "'");} ;

custom_block_start_tag:		LABEL;

//custom_block_end_tag:		{ _localctx.GetText().Equals("end") + ??? }? ENDLABEL;

custom_block_end_tag:		ENDLABEL;

customtagblock_expr:		outputexpression;

custom_blocktag_block: block* ;

custom_tag:			TAGSTART tagname customtag_expr* TAGEND ;	

customtag_expr:		outputexpression;

break_tag:			TAGSTART BREAK_TAG TAGEND ;

continue_tag:		TAGSTART CONTINUE_TAG TAGEND ;

unless_tag:			TAGSTART UNLESS_TAG if_tag_contents ENDUNLESS_TAG TAGEND ;

if_tag:				TAGSTART IF_TAG if_tag_contents ENDIF_TAG TAGEND ;

if_tag_contents:	ifexpr TAGEND ifblock elsif_tag*  else_tag? TAGSTART ;

elsif_tag:			TAGSTART ELSIF_TAG ifexpr TAGEND block*;

else_tag:			TAGSTART ELSE_TAG TAGEND block*;

ifexpr:				expr ;

ifblock:			block*;

case_tag:			TAGSTART CASE_TAG outputexpression TAGEND whenblock case_tag_contents TAGSTART ENDCASE_TAG TAGEND ;

case_tag_contents:	when_tag* when_else_tag? ;

when_else_tag:		TAGSTART ELSE_TAG TAGEND block*;

when_tag:			TAGSTART WHEN_TAG when_expressions TAGEND whenblock;

when_expressions:	outputexpression ((COMMA | OR) outputexpression)*;

whenblock:			block*;

cycle_tag:			TAGSTART CYCLE_TAG cycle_group? cycle_value (COMMA cycle_value)* TAGEND ;

cycle_group:		(STRING | variable | NUMBER) COLON ;

cycle_value:		STRING | variable | NUMBER | BOOLEAN | NULL;

assign_tag :		TAGSTART ASSIGN_TAG LABEL ASSIGNEQUALS outputexpression TAGEND ;

capture_tag :		TAGSTART CAPTURE_TAG LABEL TAGEND capture_block TAGSTART ENDCAPTURE_TAG TAGEND;

capture_block:		block* ;

include_tag :		TAGSTART INCLUDE_TAG outputexpression (include_with | include_for | include_param_pair* ) TAGEND ;

include_with :		WITH outputexpression;

include_for :		FOR_TAG outputexpression;

include_param_pair : LABEL COLON outputexpression;

for_tag:			TAGSTART FOR_TAG for_label FOR_IN for_iterable for_params* TAGEND for_block for_else? TAGSTART ENDFOR_TAG TAGEND ;

for_else:			TAGSTART ELSE_TAG TAGEND block* ;

for_block:			block* ;

for_params: 		PARAM_REVERSED | for_param_offset | for_param_limit ; // todo: limit to one of each?

for_param_offset:	PARAM_OFFSET COLON NUMBER ;

for_param_limit:	PARAM_LIMIT COLON NUMBER ;

for_label:			LABEL ;

for_iterable:		variable | STRING  | generator;

variable:			LABEL objectvariableindex* ;

generator:			PARENOPEN generator_index GENERATORRANGE generator_index PARENCLOSE ;

generator_index:	NUMBER | variable;

//comment_tag:		TAGSTART COMMENT_TAG TAGEND rawtext TAGSTART ENDCOMMENT_TAG TAGEND ;

increment_tag:		TAGSTART INCREMENT_TAG LABEL TAGEND ;

decrement_tag:		TAGSTART DECREMENT_TAG LABEL TAGEND ;

macro_tag:			TAGSTART MACRO_TAG macro_label macro_param* TAGEND macro_block TAGSTART ENDMACRO_TAG TAGEND ;

macro_param:		LABEL ;

macro_block:		block* ;

macro_label:		LABEL ;


// {{ Parse output and filters }}

outputmarkup:		OUTPUTMKUPSTART outputexpression OUTPUTMKUPEND |
					OUTPUTMKUPSTART outputexpression? {NotifyErrorListeners("Missing '}}'");};

outputexpression:	object (FILTERPIPE filter)* ; 
	
filter:				(filtername ((COLON filterargs)? 
								 | COLON {NotifyErrorListeners("Liquid error: missing arguments after colon in filter '" + _localctx.filtername().GetText() + "'");}
								 | filterargs {NotifyErrorListeners("Liquid error: missing colon before args in filter '" + _localctx.filtername().GetText() + "'");}
								 )) ;

filterargs:			filterarg (COMMA filterarg)* ;

object:				STRING									# StringObject
					| NUMBER								# NumberObject
					| BOOLEAN								# BooleanObject
					| NULL									# NullObject		
					| variable								# VariableObject
					;
 
objectvariableindex : ARRAYSTART arrayindex ARRAYEND
					//| PERIOD (objectproperty | ISEMPTY) 
					| PERIOD objectproperty
					;

					// TODO: change LABEL... to variable
arrayindex:			ARRAYINT | STRING  | LABEL objectvariableindex*  ;

objectproperty:		LABEL;

filtername:			LABEL ; 

// TODO: change LABEL... to variable

filterarg:			STRING								# StringFilterArg
					| NUMBER							# NumberFilterArg
					| BOOLEAN							# BooleanFilterArg
					| LABEL objectvariableindex*		# VariableFilterArg 
					;	 

expr:				PARENOPEN expr PARENCLOSE			# GroupedExpr 
					| outputexpression					# OutputExpression
					| NOT expr					        # NotExpr
					| expr CONTAINS expr				# ContainsExpression   // TODO: implement this
					| expr (MULT | DIV | MOD) expr      # MultExpr
					| expr (MINUS | ADD) expr           # AddSubExpr
					//| expr (NEQ | EQ) (EMPTY | NULL)    # IsEmptyOrNullExpr // TODO can 'empty' be used anywhere else? 
					| expr (GT | LT | GTE | LTE | EQ | NEQ) expr      # ComparisonExpr
					| expr AND expr                     # AndExpr
					| expr OR expr                      # OrExpr
					;	

tagname:			LABEL ; 



