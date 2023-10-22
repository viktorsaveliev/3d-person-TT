
public interface IAmmoCounter
{
    public int GetAmmoCount(AmmoData.AmmoType ammoType);
    public void SpendAmmo(AmmoData.AmmoType ammoType, int ammoCount);
}
