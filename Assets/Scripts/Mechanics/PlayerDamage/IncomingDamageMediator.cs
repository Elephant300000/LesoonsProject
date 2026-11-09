using Sasha19.Entity.MyPlayer;
using UnityEngine;
//namespase
public class IncomingDamageMediator //I â Shared
{
    public void DamageReceive(RaycastHit hit, float damage)
    {
        var receipient = hit.collider.gameObject.GetComponent<IDamageRecipient>();
        if (receipient == null) return;
        receipient.TakeDamage(damage);
    }
}
