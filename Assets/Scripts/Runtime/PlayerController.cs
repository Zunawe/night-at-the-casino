using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  public static PlayerController Instance { get; private set; }

  [SerializeField] int _maxHealth;
  [SerializeField] int _currentHealth;
  [SerializeField] float _moveSpeed;
  [SerializeField] GameObject _reticle;
  [SerializeField] ProjectileController _projectilePrefab;

  private Rigidbody2D _rigidbody;
  private Vector2 _movement;
  private int[] _dice = new int[6];

  void Awake () {
    if (Instance == null) {
      Instance = this;
      Initialize();
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    _rigidbody = GetComponent<Rigidbody2D>();
    _currentHealth = _maxHealth;
  }

  void Update () {
    
  }

  void FixedUpdate () {
    _rigidbody.velocity = _movement * _moveSpeed;
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }

  private void Initialize () {
  }

  public void OnMove (InputAction.CallbackContext context) {
    _movement = context.ReadValue<Vector2>();
    _movement.Normalize();
  }

  public void OnDash (InputAction.CallbackContext context) {
    Debug.Log("Dash");
  }

  public void OnShoot (InputAction.CallbackContext context) {
    ProjectileController projectile = Instantiate(_projectilePrefab, _reticle.transform.position, Quaternion.identity);
    projectile.Direction = _reticle.transform.localPosition;
  }

  public void OnAim (InputAction.CallbackContext context) {
    Vector2 input = context.ReadValue<Vector2>();
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(input);
    mousePosition.z = 0;

    Vector3 direction = mousePosition - transform.position;
    direction.Normalize();

    _reticle.transform.localPosition = direction;
  }

  public void OnHeal (InputAction.CallbackContext context) {
    if (context.performed) {
      if (_dice[5] > 0) {
        _dice[5] -= 1;
        _currentHealth += 1;
      }
    }
  }

  public void AddDie (int value) {
    Debug.Log("Picked up die (" + value + ")");
    ++_dice[value - 1];
  }

  public void Damage (int damage) {
    _currentHealth -= damage;

    if (_currentHealth <= 0) {
      _currentHealth = 0;
      gameObject.SetActive(false);
    }
  }

  public void Heal (int amount) {
    _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
  }
}