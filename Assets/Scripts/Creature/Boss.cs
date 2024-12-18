using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Boss : Creature
{
    public GameObject wall;
    private Coroutine detectPlayerRoutine;
    protected int currentPhase = 2;//테스트용 원래는 1
    protected int maxPhase;
    protected float maxHealthP1;
    protected float maxHealthP2;
    protected int lastPattern = -1; // 직전 패턴을 저장

    protected void SetupPhase()
    {
        switch (currentPhase)
        {
            case 1:
                curHealth = maxHealthP1;
                break;
            case 2:
                curHealth = maxHealthP2;
                break;

            default:
                break;
        }
    } //최대페이즈 추가시 case추가
    protected void CheckPhaseTransition()
    {
        if (curHealth <= 0)
        {
            TransPhase();
        }
    }
    protected void TransPhase()
    {
        if (currentPhase < maxPhase)
        {
            currentPhase++;
            SetupPhase();
        }
        else
        {
            Die();
        }
    }
    public Coroutine currentPatternCoroutine;
    protected void ExecuteCurrentPattern()
    {
        if (currentPatternCoroutine != null) return; // 중복 실행 방지
        switch (curPattern)
        {
            case 0:
                currentPatternCoroutine = StartCoroutine(ExecutePattern0());
                break;
            case 1:
                currentPatternCoroutine = StartCoroutine(ExecutePattern1());
                break;
            case 2:
                currentPatternCoroutine = StartCoroutine(ExecutePattern2());
                break;
            case 3:
                currentPatternCoroutine = StartCoroutine(ExecutePattern3());
                break;
            case 4:
                currentPatternCoroutine = StartCoroutine(ExecutePattern4());
                break;
            case 5:
                currentPatternCoroutine = StartCoroutine(ExecutePattern5());
                break;
        }
    } //보유 패턴 추가시 case추가
    protected abstract IEnumerator ExecutePattern0();
    protected abstract IEnumerator ExecutePattern1();
    protected abstract IEnumerator ExecutePattern2();
    protected abstract IEnumerator ExecutePattern3();
    protected abstract IEnumerator ExecutePattern4();
    protected abstract IEnumerator ExecutePattern5();
    protected abstract void SelectRandomPattern();//패턴 선택 매서드 추상화
    public void TrackPlayer()
    {
        if (player != null)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPosition = player.transform.position;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, step);
            transform.position = newPosition;
        }
    }
    public IEnumerator BackStep(float targetDistance)
    {
        if (player != null)
        {
            float backStepSpeed = 50f;
            Vector3 direction = (transform.position - player.transform.position).normalized; // 플레이어 반대 방향
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            float checkRadius = 1f; // 충돌 감지 반경
            LayerMask obstacleLayer = LayerMask.GetMask("Obstacle"); // 장애물 레이어

            while (true)
            {
                float currentDistance = Vector3.Distance(transform.position, player.transform.position);
                if (currentDistance >= targetDistance)
                {
                    break;
                }
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, checkRadius, direction, backStepSpeed * Time.deltaTime, obstacleLayer);
                if (hit.collider != null)
                {
                    break; // 충돌 시 백스텝 중단
                }
                // MovePosition으로 이동
                Vector3 newPosition = transform.position + direction * backStepSpeed * Time.deltaTime;
                rb.MovePosition(newPosition);
                yield return null; // 다음 프레임까지 대기
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public IEnumerator DetectPlayerCoroutine()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                wall.SetActive(true);
                Debug.Log("qkqh");
                StopDetectingPlayer();
                ChangeState(State.IDLE);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void StartDetectingPlayer()
    {
        if (detectPlayerRoutine == null)
        {
            detectPlayerRoutine = StartCoroutine(DetectPlayerCoroutine());
        }
    }

    public void StopDetectingPlayer()
    {
        if (detectPlayerRoutine != null)
        {
            StopCoroutine(detectPlayerRoutine);
            detectPlayerRoutine = null;
        }
    }
}

