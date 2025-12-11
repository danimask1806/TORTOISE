using UnityEngine;
using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;

#region Aliases
using MoveCommand = TortoiseProgram.MoveCommand;
using MoveDirection = TortoiseProgram.MoveCommand.Direction;
using RotateCommand = TortoiseProgram.RotateCommand;
using HitAction = TortoiseProgram.HitAction;
using HealAction = TortoiseProgram.HealAction;
#endregion

public class TortoiseCompiler
{
    private List<TortoiseProgram.Command> _commands = new List<TortoiseProgram.Command>();
    private List<ITortoiseAction> _stateActions = new List<ITortoiseAction>();
    private TortoiseUI _ui; // referencia a la UI para actualizar vidas

    public List<TortoiseProgram.Command> Commands => _commands;
    public List<ITortoiseAction> StateActions => _stateActions;

    // Constructor opcional con UI
    public TortoiseCompiler(TortoiseUI ui = null)
    {
        _ui = ui;
    }

    // Este método permite asignar el UI después de la compilación
    public void SetUI(TortoiseUI ui)
    {
        _ui = ui;

        // Actualizar UI en todas las acciones ya creadas
        foreach (var action in _stateActions)
        {
            if (action is HitAction hit) hit.SetUI(ui);
            if (action is HealAction heal) heal.SetUI(ui);
        }
    }

    public static TortoiseProgram Compile(string source, TortoiseUI ui)
    {
        AntlrInputStream antlerStream = new AntlrInputStream(source);
        TortoiseLexer lexer = new TortoiseLexer(antlerStream);
        CommonTokenStream tokenStream = new CommonTokenStream(lexer);
        TortoiseParser parser = new TortoiseParser(tokenStream);  

        parser.prog(); // <--- compila usando la gramática

        TortoiseCompiler compiler = parser.Compiler;
        compiler.SetUI(ui); // asignar UI real después de la compilación

        TortoiseProgram program = new TortoiseProgram(compiler.Commands);
        program.StateActions.AddRange(compiler.StateActions);

        return program;
    }

    public TortoiseCompiler AddMoveCommand(string direction, string distance)
    {
        MoveDirection dir = MoveDirection.BWD;
        if(direction.ToUpper() == "FWD") dir = MoveDirection.FWD;
        float dist = float.Parse(distance);
        _commands.Add(new MoveCommand(dir, dist));
        return this;
    }

    public TortoiseCompiler AddRotateCommand(string angle)
    {
        float a = float.Parse(angle);
        _commands.Add(new RotateCommand(a));
        return this;
    }

    public TortoiseCompiler AddHitCommand()
    {
        var hit = new HitAction(_ui);
        _stateActions.Add(hit);
        return this;
    }

    public TortoiseCompiler AddHealCommand()
    {
        var heal = new HealAction(_ui);
        _stateActions.Add(heal);
        return this;
    }
}
