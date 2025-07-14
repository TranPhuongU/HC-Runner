using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State { Idle, Running }

    [Header("Settings")]
    [SerializeField] private float searchRadius;
    [SerializeField] private float moveSpeed;
    private State state;
    private Transform targetRunner;

    [Header("Events")]
    public static Action onRunnerDied;

    void Start()
    {
        state = State.Idle; // Khởi tạo trạng thái
    }

    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        switch (state)
        {
            case State.Idle:
                SearchForTarget();
                break;

            case State.Running:
                RunTowardsTarget();
                break;
        }
    }

    private void SearchForTarget()
    {
        // Tìm tất cả collider trong bán kính
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, searchRadius);

        // Tìm runner chưa là mục tiêu
        foreach (Collider collider in detectedColliders)
        {
            if (collider.TryGetComponent(out Runner runner) && !runner.IsTarget())
            {
                runner.SetTarget();
                targetRunner = runner.transform;
                StartRunningTowardsTarget();
                return; // Thoát ngay sau khi tìm được mục tiêu
            }
        }
    }

    private void StartRunningTowardsTarget()
    {
        state = State.Running;
        GetComponent<Animator>().Play("Run");
    }

    private void RunTowardsTarget()
    {
        // Kiểm tra nếu targetRunner là null hoặc đã bị hủy
        if (targetRunner == null || targetRunner.gameObject == null)
        {
            state = State.Idle; // Quay lại trạng thái Idle để tìm mục tiêu mới
            return;
        }

        // Di chuyển về phía mục tiêu
        transform.position = Vector3.MoveTowards(transform.position, targetRunner.position,
            Time.deltaTime * moveSpeed);

        // Kiểm tra khoảng cách để hủy
        if (Vector3.Distance(transform.position, targetRunner.position) < 0.1f)
        {

            onRunnerDied?.Invoke();

            Destroy(targetRunner.gameObject);
            Destroy(gameObject);
        }
    }
}