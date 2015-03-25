lexer grammar LiquidLexer;
@lexer::header {#pragma warning disable 3021}

@lexer::members {
	public static readonly int WHITESPACE = 1;
 	int arraybracketcount = 0;

}

// MB: TODO: wait for "channels {}" syntax 
// see: https://github.com/antlr/antlr4/pull/694
COMMENT :				COMMENTSTART ( COMMENT | . )*? COMMENTEND -> skip ; // TODO: skip
fragment COMMENTSTART:	TAGSTART [ \t]* 'comment' [ \t]* TAGEND ;
fragment COMMENTEND:	TAGSTART [ \t]* 'endcomment' [ \t]* TAGEND ;

RAW :					RAWSTART RAWBLOCK RAWEND ;
RAWBLOCK:				( RAW | . )*?;
fragment RAWSTART:		TAGSTART [ \t]* 'raw' [ \t]* TAGEND ;
fragment RAWEND:		TAGSTART [ \t]* 'endraw' [ \t]* TAGEND ;

TAGSTART :				'{%'			-> pushMode(INLIQUIDTAG) ;
OUTPUTMKUPSTART :		'{{'			-> pushMode(INLIQUIDFILTER) ;
//TEXT :					.+? ;

TEXT :					.+? ;



mode NOMODE;

NUMBER :				'-'? INT '.' [0-9]+ EXP? // 1.35, 1.35E-9, 0.3, -4.5
						| '-'? INT EXP // 1e10 -3e4
						| '-'? INT // -3, 45
						;

INT :			'0' | [1-9] [0-9]* ; // no leading zeros
fragment EXP :			[Ee] [+\-]? INT ; // \- since - means "range" inside [...]

BOOLEAN :				'true' | 'false' ;

STRING :				'"' STRDOUBLE '"' | '\'' STRSINGLE '\'' ;  
fragment STRDOUBLE :	(ESC | ~["\\])* ;
fragment STRSINGLE :	(ESC | ~['\\])* ;
fragment ESC :			'\\' (["\\/bfnrt] | UNICODE) ;
fragment UNICODE :		'u' HEX HEX HEX HEX ;
fragment HEX :			[0-9a-fA-F] ;

LABEL :					ALPHA (ALPHA|DIGIT|UNDERSCORE)* ;

fragment UNDERSCORE:	'_' ;
fragment ALPHA:			[a-zA-Z] ;
fragment DIGIT:			[0-9] ;

FILTERPIPE :			'|' ;
FILTERCOLON :			':' ;

PERIOD:					'.' ;

ARRAYSTART :			'[' ;
ARRAYEND :				']' ;

GENERATORSTART : 		'(';
GENERATOREND :			')';
GENERATORRANGE :		'..';

// ========= COMMENT ===================

mode INCOMMENT;

NESTEDCOMMENT :			'{%' [ \t]+ 'comment' [ \t]+ '%}'  -> pushMode(INCOMMENT);
COMMENT_END:			'{%' [ \t]+ 'endcomment' [ \t]+ '%}' -> popMode ;

TEXT1 :					.+?  -> channel(HIDDEN);

// ========= LIQUID FILTERS ============

mode INLIQUIDFILTER ;

OUTPUTMKUPEND :			'}}'            -> popMode ;
FILTERPIPE1 :			FILTERPIPE -> type(FILTERPIPE) ;
FILTERCOLON1 :			FILTERCOLON -> type(FILTERCOLON) ;

PERIOD1:				PERIOD -> type(PERIOD) ;

NUMBER1:				NUMBER -> type(NUMBER);

BOOLEAN1:				BOOLEAN -> type(BOOLEAN);

STRING1:				STRING -> type(STRING);

LABEL1:					LABEL -> type(LABEL);

ARRAYSTART1 :			'[' -> pushMode(INARRAYINDEX), type(ARRAYSTART) ;
ARRAYEND1 :				']' -> type(ARRAYEND);



WS :					[ \t]+ -> skip ;


mode INARRAYINDEX ;

ARRAYSTART2 :			ARRAYSTART {arraybracketcount++; System.Console.WriteLine("** Encountered nested '[' " +arraybracketcount);} -> type(ARRAYSTART);
ARRAYEND2a :				{arraybracketcount == 0; }? ARRAYEND {System.Console.WriteLine("** leaving mode " +arraybracketcount);} -> type(ARRAYEND), popMode ;
ARRAYEND2b :				{arraybracketcount > 0; }? ARRAYEND  { arraybracketcount--; System.Console.WriteLine("* closed nested ']' " +arraybracketcount); } -> type(ARRAYEND);
ARRAYINT:				'0' | [1-9] [0-9]* ;
STRING3:				STRING -> type(STRING);
LABEL3:					LABEL -> type(LABEL);

// ========= LIQUID TAGS ============

mode INLIQUIDTAG ;

TAGEND :				'%}'			-> popMode ;

//COMMENT_TAG :			'comment' TAGEND -> pushMode(INCOMMENT);
//ENDCOMMENT_TAG :		'endcomment' ;
INCLUDE_TAG :			'include' ;
IF_TAG :				'if' ;
UNLESS_TAG :			'unless' ;
ELSIF_TAG :				'elsif' ;
ELSE_TAG :				'else' ;
ENDIF_TAG :				'endif' ;
ENDUNLESS_TAG :			'endunless' ;

FOR_TAG :				'for' ;
FOR_IN :				'in';
PARAM_REVERSED:			'reversed';
PARAM_OFFSET:		    'offset' ;
PARAM_LIMIT:		    'limit' ;
ENDFOR_TAG :			'endfor' ;

CYCLE_TAG :				'cycle' ;

ASSIGN_TAG :			'assign';

CAPTURE_TAG :			'capture';
ENDCAPTURE_TAG :		'endcapture';

INCREMENT_TAG :			'increment';
DECREMENT_TAG :			'decrement';

COLON :					':' ;
COMMA :					',' ;
ARRAYSTART3 :			'[' -> pushMode(INARRAYINDEX), type(ARRAYSTART) ;
ARRAYEND3 :				']' -> type(ARRAYEND);

ASSIGNEQUALS :			'=' ;

PARENOPEN :				'(' ;
PARENCLOSE :			')' ;

GT:						'>';
GTE:					'>=';
EQ:						'==';
NEQ:					'!=';
LT:						'<';
LTE:					'<=';
CONTAINS:				'contains';
AND:					'and';
OR:						'or';

MULT:					'*' ;
DIV:					'/' ;
MOD:					'%' ;
ADD:					'+' ;
SUB:					'-' ;

NUMBER2 :				NUMBER -> type(NUMBER);

BOOLEAN2 :				BOOLEAN -> type(BOOLEAN);

NEGATIVE :				'-' ;

NOT :					'!' ;

FILTERPIPE2 :			FILTERPIPE -> type(FILTERPIPE) ;
FILTERCOLON2 :			FILTERCOLON -> type(FILTERCOLON) ;
PERIOD2 :				PERIOD -> type(PERIOD) ;
//fragment ALPHACHAR:		[a-zA-Z] ;

//VARIABLE :				ALPHACHAR (ALPHACHAR | '_' | [0-9])* ;

//TRING2 :				'"' (ESC | ~["\\])* '"' ;  // TODO: accept single quotes
STRING2:				STRING -> type(STRING);

LABEL2:					LABEL -> type(LABEL);

GENERATORRANGE1:		GENERATORRANGE -> type(GENERATORRANGE) ;
//INT1:					INT -> type(INT) ;

WS2 :					[ \t]+ -> skip ;