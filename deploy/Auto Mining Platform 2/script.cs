/*
 * Automatic Mining Platform 2 By Kezeslabas
 * v0.0.1
 * 
 * -------------------------------------
 *   DO NOT MODIFY ANYTHING BELOW THIS
 * -------------------------------------
 */

readonly BlockManager BLOCKMANAGER = new BlockManager();
readonly StateManager STATEMANAGER = new StateManager();

readonly MessageQueue DEBUGQUEUE = new MessageQueue("Debug");

readonly ScriptConfig CONFIG;

readonly ScreenMessage SCREENMESSAGE;
readonly ScreenManager SCREENS;

public Program()
{
    CONFIG = new ScriptConfig(DEBUGQUEUE);
    SCREENMESSAGE = new ScreenMessage(DEBUGQUEUE,
                                      CONFIG);

    SCREENS = new ScreenManager(CONFIG,
                                STATEMANAGER,
                                SCREENMESSAGE,
                                GetMyScreen());

    BLOCKMANAGER.RegisterBlockConsumer(SCREENS);

    STATEMANAGER.RegisterStateConsumer(SCREENS);

    RefreshBlocks();
    DisplayMessages();
}

public void Save()
{

}

public void Main(string argument, UpdateType updateSource)
{
    if ((updateSource & UpdateType.Update100) != 0)
    {
    }
    else
    {
    }

    DisplayMessages();
}

private void RefreshBlocks()
{
    BLOCKMANAGER.ClearAll();

    GridTerminalSystem.SearchBlocksOfName(CONFIG.MainTag, BLOCKMANAGER.GetBlockList());

    BLOCKMANAGER.ConsumeAll();
}

private void DisplayMessages()
{
    if(CONFIG.UpdateDetailedInfo)Echo(SCREENS.CurrentMessage());
    SCREENS.DisplayOnAll();
}

private IMyTextSurface GetMyScreen()
{
    return Me.SurfaceCount >= 1 ? Me.GetSurface(0) : null;
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

public class ConfigBuilder
{
    private StringBuilder builder = new StringBuilder();

    public ConfigBuilder New()
    {
        builder.Clear();
        return this;
    }

    public ConfigBuilder Header(string header)
    {
        builder.Append("[" + header + "]\n");
        return this;
    }

    public ConfigBuilder Section(string section)
    {
        builder.Append("\n[" + section + "]\n");
        return this;
    }

    public ConfigBuilder Value<T>(string key, T value)
    {
        builder.Append(key + "=" + value.ToString() + "\n");
        return this;
    }

    public ConfigBuilder Comment(string comment)
    {
        builder.Append(";" + comment + "\n");
        return this;
    }

    public ConfigBuilder NewLine()
    {
        builder.Append("\n");
        return this;
    }

    public ConfigBuilder End()
    {
        builder.Append("\n---");
        return this;
    }

    public string Build()
    {
        return builder.ToString();
    }
}

public class ConfigInitializer
{
    protected readonly IMessageQueueAppender DebugQueue;
    private readonly MyIni INI = new MyIni();

    private bool hardChange;

    public ConfigInitializer(IMessageQueueAppender debugQueue)
    {
        DebugQueue = debugQueue;
    }

public bool IsHardChange()
    {
        return hardChange;
    }

protected bool InitFromCustomData(string customData)
    {
        INI.Clear();
        hardChange = false;
        MyIniParseResult _iniResult;
        if (customData == "" || !INI.TryParse(customData, out _iniResult))
        {
            DebugQueue.Append("Cannot read Custom Data!");
            return false;
        }
        return true;
    }

public MyIniValue Get(string section, string field)
    {
        return INI.Get(section, field);
    }

    public bool CheckIfEmpty(MyIniValue iniVal)
    {
        if (iniVal.IsEmpty)
        {
            DebugQueue.Append(iniVal.Key + " not found in Config!");
            return true;
        }
        return false;
    }

public void SoftChange<T>(ref T originalVal, T newVal)
    {
        originalVal = newVal;
    }

public void HardChange<T>(ref T originalVal, T newVal) where T : IEquatable<T>
    {
        if (!originalVal.Equals(newVal))
        {
            originalVal = newVal;
            hardChange = true;
        }
    }

public void SoftChangeBoolean(ref bool originalVal, MyIniValue iniVal)
    {
        if (!CheckIfEmpty(iniVal))
        {
            bool _bool;
            if (!Boolean.TryParse(iniVal.ToString(), out _bool))
            {
                DebugQueue.Append("Cannot Parse " + iniVal.Key);
                return;
            }
            SoftChange(ref originalVal, _bool);
        }
    }

public void HardChangeString(ref string originalVal, MyIniValue iniVal)
    {
        if (!CheckIfEmpty(iniVal))
        {
            HardChange(ref originalVal, iniVal.ToString());
        }
    }
}

public class IndicatorInfo
{
    private bool indicator = false;

    public string Build(string info)
    {
        string result = indicator ? "[-/-/-/]" : "[/-/-/-]";
        indicator = !indicator;

        result += " " + info + "\n";

        return result;
    }
}

public interface IMessageQueueAppender
{
    void Append(string msg);
}

public class MessageQueue : IMessageQueueAppender
{
    private readonly List<string> messages = new List<string>();
    private readonly string name;

    public MessageQueue(string name)
    {
        this.name = name;
    }

    public void Append(string msg)
    {
        messages.Add(msg);
    }

    public void Clear()
    {
        messages.Clear();
    }

public string ConsumeAll()
    {
        string result = "";

        messages.ForEach(p => result += "["+name+"]: " + p + "\n");
        messages.Clear();

        return result;
    }
}

public class PlayerConfig : ConfigInitializer
{
    private readonly ConfigBuilder Builder = new ConfigBuilder();

    public PlayerConfig(IMessageQueueAppender debugQueue) : base(debugQueue)
    {
        AllToDefault();
    }

    private readonly string MINIG_PLATFORM_CONFIGURATION = "Mining Platform Configuration";

    private readonly string HIGHLIGHTED_SETTINGS = "Highlighted Settings (Hard)";
    public string MainTag;

    private readonly string DISPLAY_SETTINGS = "Display Settings (Soft)";
    public bool ShowPlatformName;
    public bool LcdColorCoding;
    public bool UpdateDetailedInfo;
    public bool ShowAdvancedData;

    private readonly string TAG_SETTINGS = "Tag Settings (Hard)";
    public string VerTag;
    public string HorTag;
    public string InvTag;

    public string StartTimerTag;
    public string PauseTimerTag;
    public string FinishTimerTag;
    public string AutoStartTimerTag;
    public string AutoPauseTimerTag;

    public void AllToDefault()
    {
        MainTag = "/Mine 01/";

        ShowPlatformName = true;
        LcdColorCoding = true;
        UpdateDetailedInfo = true;
        ShowAdvancedData = false;

        VerTag = "/Ver/";
        HorTag = "/Hor/";
        InvTag = "/Inv/";

        StartTimerTag = "/Start/";
        PauseTimerTag = "/Pause/";
        FinishTimerTag = "/Finish/";
        AutoStartTimerTag = "/Auto-Start/";
        AutoPauseTimerTag = "/Auto-Pause/";
}

public bool LoadPlayerConfig(string customData, bool isContextLoad = false)
    {
        if (!InitFromCustomData(customData)) return false;

        HardChangeString(ref MainTag, Get(HIGHLIGHTED_SETTINGS, "MainTag"));

        SoftChangeBoolean(ref ShowPlatformName, Get(DISPLAY_SETTINGS, "ShowPlatformName"));
        SoftChangeBoolean(ref LcdColorCoding, Get(DISPLAY_SETTINGS, "LcdColorCoding"));
        SoftChangeBoolean(ref UpdateDetailedInfo, Get(DISPLAY_SETTINGS, "UpdateDetailedInfo"));
        SoftChangeBoolean(ref ShowAdvancedData, Get(DISPLAY_SETTINGS, "ShowAdvancedData"));

        HardChangeString(ref VerTag, Get(TAG_SETTINGS, "VerTag"));
        HardChangeString(ref HorTag, Get(TAG_SETTINGS, "HorTag"));
        HardChangeString(ref InvTag, Get(TAG_SETTINGS, "InvTag"));

        HardChangeString(ref StartTimerTag, Get(TAG_SETTINGS, "StartTimerTag"));
        HardChangeString(ref PauseTimerTag, Get(TAG_SETTINGS, "PauseTimerTag"));
        HardChangeString(ref FinishTimerTag, Get(TAG_SETTINGS, "FinishTimerTag"));
        HardChangeString(ref AutoStartTimerTag, Get(TAG_SETTINGS, "AutoStartTimerTag"));
        HardChangeString(ref AutoPauseTimerTag, Get(TAG_SETTINGS, "AutoPauseTimerTag"));

        if (!isContextLoad && IsHardChange())
        {
            DebugQueue.Append("Hard Changes Detected!");
            DebugQueue.Append("Use the SET command to Continue");
        }

        return true;
    }

public string BuildPlayerConfig()
    {
        return Builder.New()
            .Header(MINIG_PLATFORM_CONFIGURATION)
                .Comment(" You can Configure the script by changing the values below.")
                .Comment(" Apply the changes with the REFRESH or SET command.")
                .NewLine()
                .Comment(" SOFT: Can be changed any time.")
                .Comment(" HARD: When changed the script stops, and it has to be reset.")

            .Section(HIGHLIGHTED_SETTINGS)
                .NewLine()
                .Value("MainTag", MainTag)

            .Section(DISPLAY_SETTINGS)
                .NewLine()
                .Value("ShowPlatformName", ShowPlatformName)
                .Value("LcdColorCoding", LcdColorCoding)
                .Value("UpdateDetailedInfo", UpdateDetailedInfo)
                .Value("ShowAdvancedData", ShowAdvancedData)

            .Section(TAG_SETTINGS)
                .NewLine()
                .Value("VerTag", VerTag)
                .Value("HorTag", HorTag)
                .Value("InvTag", InvTag)
                .NewLine()
                .Value("StartTimerTag", StartTimerTag)
                .Value("PauseTimerTag", PauseTimerTag)
                .Value("FinishTimerTag", FinishTimerTag)
                .Value("AutoStartTimerTag", AutoStartTimerTag)
                .Value("AutoPauseTimerTag", AutoPauseTimerTag)

            .End()
            .Build();
    }
}

public class ScreenManager : IBlockConsumer, IStateConsumer
{
    private readonly ScriptConfig config;
    private readonly IStateProvider stateProvider;

    private readonly HashSet<IMyTextSurface> displays = new HashSet<IMyTextSurface>();

    private readonly IMyTextSurface coreDisplay;

    private readonly TriggeredState messageCycle = new TriggeredState();
    private readonly TriggeredState stateChange = new TriggeredState();

    private readonly IScreenMessage screenMessage;

    private string currentMessage;

    public ScreenManager(ScriptConfig config, IStateProvider stateProvider, IScreenMessage screenMessage ,IMyTextSurface coreDisplay)
    {
        this.config = config;
        this.stateProvider = stateProvider;

        this.screenMessage = screenMessage;
        this.coreDisplay = coreDisplay;
        AddCoreDisplay();
    }

public void DisplayOnAll()
    {
        CurrentMessage();

        if (stateChange.IsTriggered())
        {
            foreach (IMyTextSurface surface in displays)
            {
                SetDisplayColor(surface);
                surface.WriteText(currentMessage);
            }
        }
        else
        {
            foreach (IMyTextSurface surface in displays)
            {
                surface.WriteText(currentMessage);
            }
        }

        messageCycle.Continue();
        stateChange.Continue();
    }

public string CurrentMessage()
    {
        if (messageCycle.IsTriggered()) return currentMessage;

        currentMessage = screenMessage.BuildMessage(stateProvider.GetState());

        messageCycle.Trigger();
        return currentMessage;
    }

private void AddCoreDisplay()
    {
        if(coreDisplay != null)
        {
            displays.Add(coreDisplay);
        }
    }

public int GetDisplayCount()
    {
        return displays.Count();
    }

public void ClearBlocks()
    {
        displays.Clear();
        AddCoreDisplay();
    }

public void ConsumeBlock(IMyTerminalBlock block)
    {
        AddDisplay(block);
    }

    private void SetDisplayColor(IMyTextSurface textSurface)
    {
        if (config.LcdColorCoding)
        {
            textSurface.FontColor = config.STATEDATA[stateProvider.GetState()].Color;
        }
    }

private void AddDisplay(IMyTerminalBlock block)
    {
        if(block is IMyTextPanel)
        {
            AddDisplay(block as IMyTextSurface);
        }
        else if(block is IMyTextSurfaceProvider)
        {
            string stringBuffer;
            int intBuffer;
            IMyTextSurfaceProvider providerBuffer;

            Array.ForEach(block.CustomData.Split('\n'), s =>
            {
                if (s.StartsWith("@"))
                {
                    stringBuffer = s.Substring(1);
                    if(stringBuffer.Contains(config.MainTag))
                    {
                        stringBuffer = stringBuffer.Replace(config.MainTag, "");
                        if(Int32.TryParse(stringBuffer, out intBuffer))
                        {
                            providerBuffer = block as IMyTextSurfaceProvider;
                            if(providerBuffer.SurfaceCount > intBuffer)
                            {
                                AddDisplay(providerBuffer.GetSurface(intBuffer));
                            }
                        }
                    }
                }
            });
        }
    }

private void AddDisplay(IMyTextSurface textSurface)
    {
        textSurface.ContentType = ContentType.TEXT_AND_IMAGE;
        SetDisplayColor(textSurface);
        displays.Add(textSurface);
    }

    public void ConsumeState(ScriptState state)
    {
        stateChange.Trigger();
    }
}
public interface IScreenMessage
{
    IMessageQueueAppender GetInfoQueueAppender();
    IMessageQueueAppender GetWarningQueueAppender();
    string BuildMessage(ScriptState state);
}
public class ScreenMessage : IScreenMessage
{
    private readonly ScriptConfig config;

    private readonly MessageQueue debugQueue;
    private readonly IndicatorInfo indicatorBar = new IndicatorInfo();

    private readonly MessageQueue infoQueue = new MessageQueue("Info");
    private readonly MessageQueue warningQueue = new MessageQueue("Warning");

    public ScreenMessage(MessageQueue debugQueue, ScriptConfig config)
    {
        this.debugQueue = debugQueue;
        this.config = config;
    }

    public IMessageQueueAppender GetInfoQueueAppender()
    {
        return infoQueue;
    }

    public IMessageQueueAppender GetWarningQueueAppender()
    {
        return warningQueue;
    }

public string BuildMessage(ScriptState state)
    {
        string result = "";
        result += debugQueue.ConsumeAll();
        if (config.ShowPlatformName) result += "Platform: " + config.MainTag + "\n";
        result += indicatorBar.Build(config.STATEDATA[state].Text);

        result += warningQueue.ConsumeAll();
        result += infoQueue.ConsumeAll();

        return result;
    }

}

public enum ScriptState
{
    UNDEFINED,
    INIT,
    SET
}

public class ScriptConfig : PlayerConfig
{
    public readonly Dictionary<ScriptState, StateData> STATEDATA = new Dictionary<ScriptState, StateData>
    {
        { ScriptState.UNDEFINED,    new StateData(Color.Gray,          "Undefined State")   },
        { ScriptState.INIT,         new StateData(Color.White,         "Initializing...")   },
        { ScriptState.SET,          new StateData(Color.Magenta,       "Set")               },
    };

    public ScriptConfig(IMessageQueueAppender debugQueue) : base(debugQueue)
    {

    }
}

public struct StateData
{
    public Color Color { get; }
    public string Text { get; }

    public StateData(Color color, string text)
    {
        Color = color;
        Text = text;
    }
}

public interface IStateProvider
{
    void SetState(ScriptState state);

    ScriptState GetState();
}

public interface IStateConsumer
{
    void ConsumeState(ScriptState state);
}


public class StateManager : IStateProvider
{
    private readonly List<IStateConsumer> consumers = new List<IStateConsumer>();

    private ScriptState state;

    public ScriptState GetState()
    {
        return state;
    }

    public void SetState(ScriptState state)
    {
        this.state = state;
        CallConsumers();
    }

    public void RegisterStateConsumer(IStateConsumer consumer)
    {
        consumers.Add(consumer);
    }

    private void CallConsumers()
    {
        consumers.ForEach(p => p.ConsumeState(state));
    }
}

public class TriggeredState
{
    private bool triggered = false;

    public void Continue()
    {
        triggered = false;
    }

    public void Trigger()
    {
        triggered = true;
    }

    public bool IsTriggered()
    {
        return triggered;
    }
}