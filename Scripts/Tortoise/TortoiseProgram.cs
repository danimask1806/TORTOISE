using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region Interfaces
public interface ITortoiseAction
{
    IEnumerator Execute(GameObject obj, TortoiseState state);
}
#endregion

#region State Class
public class TortoiseState
{
    public Vector3 Position;
    public Quaternion Rotation;
    public int Lives;

    public TortoiseState(Vector3 pos, Quaternion rot, int lives)
    {
        Position = pos;
        Rotation = rot;
        Lives = lives;
    }
}
#endregion

public class TortoiseProgram
{
    #region Command Interfaces
    public interface Command
    {
        IEnumerator Execute(GameObject obj);
    }
    #endregion

    #region Actions
    public List<ITortoiseAction> StateActions = new List<ITortoiseAction>();

    public class MoveCommand : Command
    {
        public enum Direction { FWD, BWD }
        private Direction _direction;
        private float _distance;

        public MoveCommand(Direction dir, float dist)
        {
            _direction = dir;
            _distance = dist;
        }

        public IEnumerator Execute(GameObject obj)
        {
            float d = (_direction == Direction.FWD) ? _distance : -_distance;
            obj.transform.Translate(Vector3.up * d);
            yield return null;
        }
    }

    public class RotateCommand : Command
    {
        private float _angle;

        public RotateCommand(float angle)
        {
            _angle = angle;
        }

        public IEnumerator Execute(GameObject obj)
        {
            obj.transform.Rotate(new Vector3(0, 0, _angle));
            yield return null;
        }
    }

    public class HitAction : ITortoiseAction
    {
        private TortoiseUI _ui;

        public HitAction(TortoiseUI ui)
        {
            _ui = ui;
        }

        public void SetUI(TortoiseUI ui)
        {
            _ui = ui;
        }

        public IEnumerator Execute(GameObject obj, TortoiseState state)
        {
            state.Lives--;
            Debug.Log("Tortuga dañada. Vidas = " + state.Lives);

            _ui?.UpdateLives(); // actualiza UI si existe
            yield return null;
        }
    }

    public class HealAction : ITortoiseAction
    {
        private TortoiseUI _ui;

        public HealAction(TortoiseUI ui)
        {
            _ui = ui;
        }

        public void SetUI(TortoiseUI ui)
        {
            _ui = ui;
        }

        public IEnumerator Execute(GameObject obj, TortoiseState state)
        {
            state.Lives++;
            Debug.Log("Tortuga curada. Vidas = " + state.Lives);

            _ui?.UpdateLives(); // actualiza UI si existe
            yield return null;
        }
    }
    #endregion

    #region Program Execution
    public static float EXECUTION_DELAY = 0.5f;
    private List<Command> _commands;
    private int _pc;

    public TortoiseProgram(List<Command> commands)
    {
        _commands = commands;
        _pc = 0;
    }

    public IEnumerator Run(GameObject obj)
	{
		while (_pc < _commands.Count)
		{
			yield return new WaitForSeconds(EXECUTION_DELAY);
			Command nextCommand = _commands[_pc++];
			yield return nextCommand.Execute(obj);
		}

		RuntimeScriptable rs = obj.GetComponent<RuntimeScriptable>();
		TortoiseState state = rs != null ? rs.State : new TortoiseState(obj.transform.position, obj.transform.rotation, 3);

		foreach (var action in StateActions)
		{
			yield return new WaitForSeconds(EXECUTION_DELAY);
			yield return action.Execute(obj, state);
		}
	}


    public void Reset()
    {
        _pc = 0;
    }
    #endregion
}
