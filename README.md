# AntlrExampleUnity (Basado en [AntlrExampleUnity de jewagenaar](https://github.com/jewagenaar/AntlrExampleUnity))

## Descripción

Este proyecto es una extensión del [AntlrExampleUnity](https://github.com/jewagenaar/AntlrExampleUnity) original creado por [jewagenaar](https://github.com/jewagenaar). Demuestra cómo implementar un lenguaje de secuencias creado con Antlr en Unity. El proyecto incluye una escena de demostración (TortoiseScene) que muestra el uso del lenguaje de secuencias basado en Antlr en un entorno de juego de Unity.

## Propósito

La motivación detrás de este proyecto es explorar la integración de Antlr en Unity para crear un lenguaje de secuencias simple. El lenguaje de secuencias está diseñado para manipular GameObjects en tiempo de ejecución, proporcionando una base para incorporar elementos de codificación en la jugabilidad.

## Cómo Usar

### Requisitos Previos

Antes de usar este proyecto, asegúrate de tener Antlr instalado. Los siguientes pasos te guiarán a través del proceso de instalación:

1. Navega al directorio `/usr/local/lib`.
2. Descarga el archivo JAR completo de Antlr:
 ```  $ curl -O http://www.antlr.org/download/antlr-4.5-complete.jar```
### Establece la variable de entorno CLASSPATH:

```$ export CLASSPATH=".:/usr/local/lib/antlr-4.5-complete.jar:$CLASSPATH"```


### Crea un alias para Antlr4:


```$ alias antlr4='java -Xmx500M -cp "/usr/local/lib/antlr-4.5-complete.jar:$CLASSPATH" org.antlr.v4.Tool'```

### Integración con Unity

Descarga la ejecución de C# para Antlr y descomprímela.
Arrastra y suelta los archivos de ejecución de C# en tu proyecto de Unity.

### Gramática de Antlr

Crea un archivo de gramática Antlr llamado "Hello.g4" en tu carpeta Assets.
Copia y pega las siguientes reglas de gramática en "Hello.g4":


```grammar Hello; ```

```
r  : 'hello' ID { UnityEngine.Debug.Log("Antlr dice: ¡Hola, " + $ID.text); } ;  // coincide con la palabra clave hello seguida de un identificador 
ID : ([A-Z] | [a-z])+ ; // coincide con identificadores en minúsculas
WS : [ \t\r\n]+ -> skip ; // omite espacios, tabulaciones y saltos de línea
```
### Genera los archivos fuente en C# ejecutando el siguiente comando en tu terminal:

```antlr4 -Dlanguage=CSharp Hello.g4```
### Script de Unity
Crea un nuevo GameObject en tu escena de Unity.

Agrega un nuevo script llamado "HelloAntlr" al GameObject.

Copia y pega el siguiente script en "HelloAntlr":


```
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Collections;

public class HelloAntlr : MonoBehaviour 
{
    public string Name = "mundo";

    void Start () 
    {
        AntlrInputStream antlerStream = new AntlrInputStream("hello " + Name);
        HelloLexer lexer = new HelloLexer(antlerStream);
        CommonTokenStream tokenStream = new CommonTokenStream(lexer);
        HelloParser parser = new HelloParser(tokenStream);

        parser.r();
    }
}

```

Haz clic en el botón de reproducción en Unity para ver la salida en la consola: "Antlr dice: ¡Hola, Mundo!"

Demostración de Tortoise
El proyecto incluye una escena de demostración, "TortoiseScene," que ilustra cómo un lenguaje de secuencias creado con Antlr puede usarse en un juego de Unity. El lenguaje se utiliza para controlar el movimiento y la rotación de un sprite de Tortoise en la pantalla en tiempo de ejecución, usando comandos simples como mov (mover) y rot (rotar).

Ejemplo de script en el lenguaje Tortoise:

```

mov fwd 3
rot 30
mov fwd 2
hit
rot -45
mov bwd 1
heal
mov fwd 2

```
