using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterBehavior
{
    Ambient,
    Aggressive,
    Neutral
}

public enum CharacterState
{
    Idle,
    Attacking
}

public class Character : MonoBehaviour
{

    public Player rider = null;

    public bool isRidden = false;

    public float health;
    public float maxHealth;
    public float damage;

    public float attackCooldown = 0f;

    public CharacterBehavior behavior = CharacterBehavior.Ambient;

    public GameObject mountPoint;

    public MeshRenderer meshRenderer;

    private MaterialPropertyBlock block;

    public AudioSource hurtSound;
    public AudioSource deathSound;

    private Color baseColor = Color.white;
    private float flashTime = 1f;
    private float timer;

    protected virtual void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(block);
        baseColor = meshRenderer.material.color;
        block.SetColor("_BaseColor", baseColor);
        meshRenderer.SetPropertyBlock(block);
    }

    public void Mount(Player p)
    {
        if (rider == null)
        {
            rider = p;
            isRidden = true;
            p.transform.SetParent(transform);
            p.transform.localPosition = mountPoint.transform.localPosition;
            p.transform.localRotation = mountPoint.transform.localRotation;
            p.playerRigidbody.isKinematic = true;
        }
    }
    public void Dismount()
    {
        if (rider != null)
        {
            rider.transform.SetParent(null);
            rider.playerRigidbody.isKinematic = false;
            rider = null;
            isRidden = false;
        }
    }

    public void Hurt(float level)
    {
        timer = flashTime;
        health -= level;
        if (health <= 0f)
        {
            Die();
        }
        hurtSound.Play();
    }

    protected virtual void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            Color flashColor = Color.Lerp(baseColor, Color.red, Mathf.PingPong(Time.time * 10, 1f));
            block.SetColor("_BaseColor", flashColor);
            meshRenderer.SetPropertyBlock(block);
        }
        else
        {
            block.SetColor("_BaseColor", baseColor);
            meshRenderer.SetPropertyBlock(block);
        }
    }

    public CharacterState state = CharacterState.Idle;

    public void Attack(Character target)
    {
        if (behavior == CharacterBehavior.Ambient)
        {
            return;
        }
        if (attackCooldown > 0f)
        {
            return;
        }
        state = CharacterState.Attacking;
        target.Hurt(damage);
        if (behavior == CharacterBehavior.Neutral)
        {
            state = CharacterState.Idle;
        }
        attackCooldown = 2f;
        StartCoroutine(WaitCool());
    }

    public void Attack(Player target)
    {
        if (behavior == CharacterBehavior.Ambient)
        {
            return;
        }
        if (attackCooldown > 0f)
        {
            return;
        }
        state = CharacterState.Attacking;
        target.Hurt(damage);
        if (behavior == CharacterBehavior.Neutral)
        {
            state = CharacterState.Idle;
        }
        attackCooldown = 2f;
        StartCoroutine(WaitCool());
    }

    public void Die()
    {
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        deathSound.Play();
        yield return new WaitForSeconds(deathSound.clip.length + 0.1f);
        Destroy(gameObject);
        yield break;
    }

    IEnumerator WaitCool()
    {
        while (true)
        {
            if (attackCooldown <= 0f)
            {
                yield break;
            }
            else
            {
                attackCooldown -= 1f;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}