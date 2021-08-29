/*
 * R e a d m e
 * -----------
 * 
 * In this file you can include any instructions or other comments you want to have injected onto the 
 * top of your final script. You can safely delete this file if you do not want any such comments.
 */

//readonly DebugMessage DEBUG_MESSAGE = new DebugMessage();
readonly BlockManager BLOCK_MANAGER = new BlockManager();

public Program()
{
    Init();
}

public void Save()
{

}

public void Main(string argument, UpdateType updateSource)
{
    if ((updateSource & UpdateType.Update100) != 0)
    {
        //Automatic run
    }
    else
    {
        //Manual Run
    }
}

public void Init()
{

}

public void RefreshBlocks()
{
    BLOCK_MANAGER.ClearAll();

    GridTerminalSystem.SearchBlocksOfName("MAIN TAG", BLOCK_MANAGER.GetBlockList());

    BLOCK_MANAGER.ConsumeAll();
}

public interface IBlockConsumer
{
    void ConsumeBlock(IMyTerminalBlock block);
    void ClearBlocks();
}
public class BlockManager
{
    private readonly List<IBlockConsumer> blockConsumers = new List<IBlockConsumer>();
    private readonly List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

    public void ClearAll()
    {
        blocks.Clear();
        blockConsumers.ForEach(p => p.ClearBlocks());
    }

    public void RegisterBlockConsumer(IBlockConsumer blockConsumer)
    {
        blockConsumers.Add(blockConsumer);
    }

    public List<IMyTerminalBlock> GetBlockList()
    {
        return blocks;
    }

    public void ConsumeAll()
    {
        blocks.ForEach(p => blockConsumers.ForEach(k => k.ConsumeBlock(p)));
    }
}