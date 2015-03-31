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
					| custom_tag
					// | raw_tag
					| if_tag
					| for_tag
					| cycle_tag // you can use this outside a for loop.
					| assign_tag
					| capture_tag
					| increment_tag
					| decrement_tag
					| unless_tag
					
					| case_tag
					// | table_tag
					// | include_tag
					| break_tag
					| continue_tag
					;

// text wrapped in a raw tag
raw_tag:			RAW;

//raw_tag:			RAW_START .*? RAW_END;

custom_tag:			TAGSTART tagname outputexpression* TAGEND ;	

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

when_tag:			TAGSTART WHEN_TAG outputexpression TAGEND whenblock;

whenblock:			block*;

cycle_tag:			TAGSTART CYCLE_TAG cycle_group? cycle_string (COMMA cycle_string)* TAGEND ;

cycle_group:		STRING COLON ;

cycle_string:		STRING;

assign_tag :		TAGSTART ASSIGN_TAG LABEL ASSIGNEQUALS outputexpression TAGEND ;

capture_tag :		TAGSTART CAPTURE_TAG LABEL TAGEND capture_block TAGSTART ENDCAPTURE_TAG TAGEND;

capture_block:		block* ;

for_tag:			TAGSTART FOR_TAG for_label FOR_IN for_iterable for_params* TAGEND for_block TAGSTART ENDFOR_TAG TAGEND ;

for_block:			block* ;

for_params: 		PARAM_REVERSED | for_param_offset | for_param_limit ; // todo: limit to one of each?

for_param_offset:	PARAM_OFFSET COLON NUMBER ;

for_param_limit:	PARAM_LIMIT COLON NUMBER ;

for_label:			LABEL ;

for_iterable:		variable | STRING  | generator;

variable:			LABEL objectvariableindex* ;

generator:			PARENOPEN (NUMBER | variable) GENERATORRANGE (NUMBER | variable) PARENCLOSE ;

//comment_tag:		TAGSTART COMMENT_TAG TAGEND rawtext TAGSTART ENDCOMMENT_TAG TAGEND ;

increment_tag:		TAGSTART INCREMENT_TAG LABEL TAGEND ;

decrement_tag:		TAGSTART DECREMENT_TAG LABEL TAGEND ;

// {{ Parse output and filters }}

outputmarkup:		OUTPUTMKUPSTART outputexpression OUTPUTMKUPEND ;

outputexpression:	object (FILTERPIPE filter)* ; 
	
filter:				(filtername (COLON filterargs)?) ;

filterargs:			filterarg (COMMA filterarg)* ;

object:				STRING									# StringObject
					| NUMBER								# NumberObject
					| BOOLEAN								# BooleanObject
					//| 'nil'								# NilObject TODO						
					| variable								# VariableObject
					;
 
objectvariableindex : ARRAYSTART arrayindex ARRAYEND
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

expr:				PARENOPEN expr PARENCLOSE			# GroupedExpr // TODO is this in regular Liquid?
					| outputexpression					# OutputExpression
					| NOT expr					        # NotExpr
					//	|  typecast?
					| expr (MULT | DIV | MOD) expr      # MultExpr
					| expr (SUB | ADD) expr             # AddSubExpr
					| expr (GT | LT | GTE | LTE | EQ | NEQ) expr      # ComparisonExpr
					| expr AND expr                     # AndExpr
					| expr OR expr                      # OrExpr
					// todo: nil
					;	

tagname:			LABEL ; 

