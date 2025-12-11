using UnityEngine;

public partial class RuntimeScriptable : MonoBehaviour
{
    [Header("Vidas iniciales")]
    public int initialLives = 3;

    public TortoiseState State ;

	private Quaternion _startRotation;
	private Vector3 _startPosition;
	private TortoiseProgram _program;


	public TortoiseUI ui; 

	public void CompileAndRun(string code)
	{
		StopAllCoroutines();
		_program = TortoiseCompiler.Compile(code, ui); 
		StartCoroutine(_program.Run(gameObject));
	}


	void Start()
	{
		_startRotation = transform.rotation;
		_startPosition = transform.position;
	}

    void Awake()
    {
        State = new TortoiseState(transform.position, transform.rotation, initialLives);
    }

    public void Hit()
    {
    }

    public void Heal()
    {
    }

    public void ResetState()
    {
        StopAllCoroutines();
        transform.position = State.Position = _startPosition;
        transform.rotation = State.Rotation = _startRotation;
        State.Lives = initialLives;
        Debug.Log("Tortuga reiniciada. Vidas = " + State.Lives);
    }

}

