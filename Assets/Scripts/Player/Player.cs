using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    private float _rotationSpeed = 15f;
    private int _maxHealth = 10;
    private int _health = 10;
    private Rigidbody2D _rigidBody;
    private Vector2 _movement;
    [SerializeField] private int _power;
    [SerializeField] private Text _healthText;

    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    private KeyCode newKey;
    public Text keyBindText;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move Player
        if (Input.GetKey(leftKey))
        {
            _movement.x = -1;
        }
        else if (Input.GetKey(rightKey))
        {
            _movement.x = 1;
        }
        else
        {
            _movement.x = 0;
        }
        if (Input.GetKey(upKey))
        {
            _movement.y = 1;
        }
        else if (Input.GetKey(downKey))
        {
            _movement.y = -1;
        }
        else
        {
            _movement.y = 0;
        }

        // If any arrow key is pressed, change key for that direction
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StartCoroutine(ChangeKey(KeyCode.LeftArrow));
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StartCoroutine(ChangeKey(KeyCode.RightArrow));
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            StartCoroutine(ChangeKey(KeyCode.UpArrow));
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StartCoroutine(ChangeKey(KeyCode.DownArrow));
        }

        // Check if the player is actually moving
        if (_movement.sqrMagnitude > 0)
        {
            // Calculate the angle of movement in degrees
            float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f;
            // Create a rotation around the Z axis based on the angle
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // Set the player's rotation to the calculated rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }


        // TEST CODE Decrease the player's health by 1 when the z key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(1);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Fire();
        }
    }

    private IEnumerator ChangeKey(KeyCode arrowKey)
    {
        Time.timeScale = 0; // pause the game
        keyBindText.text = "Press new key for " + arrowKey.ToString();
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        newKey = GetNewKey();
        keyBindText.text = "";

        switch (arrowKey)
        {
            case KeyCode.LeftArrow:
                leftKey = newKey;
                break;
            case KeyCode.RightArrow:
                rightKey = newKey;
                break;
            case KeyCode.UpArrow:
                upKey = newKey;
                break;
            case KeyCode.DownArrow:
                downKey = newKey;
                break;
        }
        Time.timeScale = 1; // resume the game
    }

    // Read Key Input
    private KeyCode GetNewKey()
    {
        while (true)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    return vKey;
                }
            }
        }
    }


    void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _movement * _speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            if (_health < 0)
            {
                _health = 0;
            }
            UpdateHealthUI();
        }        
    }

    private void UpdateHealthUI()
    {
        _healthText.text = "HP " + _health + "/" + _maxHealth;
    }

    public int GetHealth()
    {
        return _health;
    }

    public void SetHealth(int newHealth)
    {
        _health = newHealth;
        UpdateHealthUI();
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        UpdateHealthUI();
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public int GetPower(){
        return _power;
    }
    public void SetPower(int newPower){
        _power = newPower;
    }
    public void Fire(){
        Physics2D.Raycast((Vector2)this.transform.position, Vector2.up);
    }
}
