
public interface ISavable
{
    public void SaveGame(GameData gameData);

    public void LoadData(GameData gameData);

    public string GetGUID();

    public void AfterAllObjectsLoaded(GameData gameData);
}