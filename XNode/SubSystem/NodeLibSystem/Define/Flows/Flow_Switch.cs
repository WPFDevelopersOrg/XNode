using XLib.Base.Ex;
using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Flows
{
    public class Flow_Switch : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Flow, "Flow", "选择执行");

            PinGroupList.Add(new ExecutePinGroup(this, "执行与值相等的引脚"));
            PinGroupList.Add(new DataPinGroup(this, "string", "值", "") { BoxWidth = 120, Readable = false });
            PinGroupList.Add(new ActionPinGroup(this, "Case_01"));
            PinGroupList.Add(new ActionPinGroup(this, "Case_02"));
            PinGroupList.Add(new ActionPinGroup(this, "Default"));

            CustomListProperty caseList = new CustomListProperty
            {
                Name = "值列表",
                ItemList = new List<string> { "Case_01", "Case_02", "Default" },
                ItemAdded = OnItemAdded,
                ItemRemoved = OnItemRemoved,
                ItemChanged = OnItemChanged
            };
            PropertyList.Add(caseList);

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 更新值
            UpdateData(1);
            // 获取动作引脚索引
            int actionIndex = GetActionIndex(GetData(1));
            // 执行引脚
            if (actionIndex != -1) GetPinGroup<ActionPinGroup>(actionIndex + 2).Invoke();

            GetPinGroup<ExecutePinGroup>().Execute();
        }

        public override string GetTypeString() => nameof(Flow_Switch);

        public override Dictionary<string, string> GetParaDict() =>
            new Dictionary<string, string> { { "Value", GetData(1) } };

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["Value"]);
            }
            catch (Exception) { }
        }

        public override Dictionary<string, string> GetPropertyDict()
        {
            return new Dictionary<string, string> { { "CustomList", PropertyList[0].ToString() } };
        }

        public override void LoadPropertyDict(string version, Dictionary<string, string> propertyDict)
        {
            try
            {
                PropertyList[0] = new CustomListProperty(propertyDict["CustomList"])
                {
                    Name = "值列表",
                    ItemAdded = OnItemAdded,
                    ItemRemoved = OnItemRemoved,
                    ItemChanged = OnItemChanged
                };
                UpdateAllItem();
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Flow_Switch();

        #region 属性变更

        private void OnItemAdded(string item)
        {
            ActionPinGroup group = new ActionPinGroup(this, item);
            PinGroupList.Add(group);
            group.Init();
            // 通知引脚组变更
            InvokePinGroupListChanged();
        }

        private void OnItemRemoved(int index)
        {
            // 引脚组索引
            int groupIndex = index + 2;
            if (PinGroupList.IndexOut(groupIndex)) return;
            // 断开引脚
            BreakPin(groupIndex);
            // 移除引脚组
            PinGroupList.RemoveAt(groupIndex);
            // 通知引脚组变更
            InvokePinGroupListChanged();
        }

        private void OnItemChanged(int index, string value)
        {
            int groupIndex = index + 2;
            if (PinGroupList.IndexOut(groupIndex)) return;
            ((ActionPinGroup)PinGroupList[groupIndex]).ActionName = value;
            // 通知引脚组变更
            InvokePinGroupListChanged();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 更新全部项
        /// </summary>
        private void UpdateAllItem()
        {
            while (PinGroupList.Count > 2) PinGroupList.RemoveLast();
            foreach (var item in ((CustomListProperty)PropertyList[0]).ItemList)
            {
                ActionPinGroup group = new ActionPinGroup(this, item);
                PinGroupList.Add(group);
                group.Init();
            }
        }

        private int GetActionIndex(string value)
        {
            int defaultIndex = ((CustomListProperty)PropertyList[0]).ItemList.IndexOf("Default");
            int valueIndex = ((CustomListProperty)PropertyList[0]).ItemList.IndexOf(value);
            return valueIndex == -1 ? defaultIndex : valueIndex;
        }

        #endregion
    }
}