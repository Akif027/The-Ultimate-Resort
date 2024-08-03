using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Employee : MonoBehaviour
{
    public enum Type { Maid, Receptionist }

    public Type EmployeeType = Type.Maid;
    public SecuredDouble Salary;
    public float PayAfterTime;
    public Transform SignPos;

    private NavMeshAgent agent;
    private RoomManager roomManager;
    private Animation animator;
    private GetMoney getMoney;
    private GameObject MoneyUI = null;
    private Vector3 DefaultPos;

    private bool isPaid;

    private void OnEnable()
    {
        DefaultPos = transform.position;
    }

    private void Start()
    {
        isPaid = true;
        roomManager = GameManager.Instance.GetManager<RoomManager>() as RoomManager;
        animator = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        getMoney = GetComponentInChildren<GetMoney>();
        StartCoroutine(PaymentCycle());
    }

    private void Update()
    {
        HandleEmployee();
        EmployeeAnimationWalkAndIdle();
    }

    private IEnumerator PaymentCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(PayAfterTime);
            UnpayEmployee();
        }
    }

    private void HandleEmployee()
    {
        if (!isPaid)
        {
            SetDestination(DefaultPos);
            if (MoneyUI != null) MoneyUI.transform.position = SignPos.position;
            return;
        }

        switch (EmployeeType)
        {
            case Type.Maid:
                HandleMaidActivities();
                break;
            case Type.Receptionist:
                HandleReceptionistActivities();
                break;
        }
    }

    private void HandleMaidActivities()
    {
        Room room = roomManager.GetUncleanedRooms();
        if (room != null && !room.IsEverythingCleaned())
        {

            SetDestination(room.GetUncleanedFurniture().transform.position);
        }
        else
        {
            SetDestination(DefaultPos);
        }
    }

    private void HandleReceptionistActivities()
    {
        // Add receptionist activities here
    }

    private void SetDestination(Vector3 destination)
    {
        agent.destination = destination;
    }

    private void EmployeeAnimationWalkAndIdle()
    {
        if (animator.CurrentState == AnimationState.Clean) return;

        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance + 0.1f)
        {
            animator.ChangeState(AnimationState.Walk);
            if (MoneyUI != null) MoneyUI.SetActive(false);
        }
        else
        {
            animator.ChangeState(AnimationState.Idle);
            if (MoneyUI != null) MoneyUI.SetActive(true);
        }

        int blendValue = getMoney.sortSlot.SortObjects.Count > 0 ? 1 : 0;
        animator.SetBlendIdle(blendValue);
        animator.SetBlendMove(blendValue);
    }

    public void PayEmployee()
    {
        if (Game.Instance.gameData.money.Round() >= Salary)
        {
            isPaid = true;
            Game.Instance.gameData.TrySpendMoney(Salary);
            getMoney.PlaceMoney();
            ObjectPool.Instance.ReturnObjectToPool(MoneyUI, "MoneySign");
            MoneyUI = null;
        }
    }

    public void UnpayEmployee()
    {
        isPaid = false;
        ShowPaymentSign();
    }

    private void ShowPaymentSign()
    {
        if (MoneyUI != null) return;
        MoneyUI = ObjectPool.Instance.GetPooledObject("MoneySign");
        MoneyUI.transform.SetPositionAndRotation(SignPos.position, SignPos.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPaid && other.CompareTag("Player"))
        {
            PayEmployee();
        }
    }
}
