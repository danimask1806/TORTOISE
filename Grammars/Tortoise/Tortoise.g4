grammar Tortoise;

@members 
{
    public TortoiseCompiler Compiler = new TortoiseCompiler();
}

// Programa principal: una o más instrucciones
prog : cmd+ ;

// Cada comando termina en nueva línea
cmd : (move | rotate | hit | heal) NEWLINE ;

// Movimiento: 'mov' seguido de dirección y valor
move : MOV DIR VAL { Compiler.AddMoveCommand($DIR.text, $VAL.text); };

// Rotación: 'rot' seguido de valor
rotate : ROT VAL { Compiler.AddRotateCommand($VAL.text); };

// Hit: resta vida
hit : HIT { Compiler.AddHitCommand(); };

// Heal: suma vida
heal : HEAL { Compiler.AddHealCommand(); };

// Tokens
MOV : 'mov' ;
ROT : 'rot' ;
HIT : 'hit' ;
HEAL : 'heal' ;

DIR : 'fwd' | 'bwd' ;
VAL : INT ;
INT : '-'? ('0'..'9')+ ;

NEWLINE : '\r'? '\n' ;
WS : [ \t\r\n]+ -> skip ;
