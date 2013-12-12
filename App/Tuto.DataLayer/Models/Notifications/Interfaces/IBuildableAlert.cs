namespace Tuto.DataLayer.Models.Notifications.Interfaces
{
    public interface IBuildableAlert<T>
    {
        T getBuilder();
    }
}