using System.Windows;
using XLib.Base;
using XLib.Node;

namespace XNode.SubSystem.NodeEditSystem.Control.PropertyBar
{
    public partial class ListEditer : PropertyBarBase
    {
        public ListEditer() => InitializeComponent();

        public CustomListProperty Instance { get; set; }

        public void Init()
        {
            Stack_CaseList.Children.Clear();
            foreach (var item in Instance.ItemList)
            {
                CustomListItem listItem = new CustomListItem
                {
                    ItemValue = item,
                    Margin = new Thickness(0, 10, 0, 0),
                    Remove_Click = OnRemoveClick,
                    Value_Changed = OnValueChanged
                };
                Stack_CaseList.Children.Add(listItem);
            }
            if (Stack_CaseList.Children.Count > 0)
                ((CustomListItem)Stack_CaseList.Children[0]).Margin = new Thickness();
            else Grid_Tool.Margin = new Thickness();
        }

        private void Tool_Add_Click(object sender, RoutedEventArgs e)
        {
            (int, int) mousePoint = OSTool.GetMousePoint();

            // 生成项
            string value = GenerateCaseValue();
            CustomListItem listItem = new CustomListItem
            {
                ItemValue = value,
                Margin = new Thickness(0, 10, 0, 0),
                Remove_Click = OnRemoveClick,
                Value_Changed = OnValueChanged
            };
            // 添加项
            Stack_CaseList.Children.Add(listItem);
            ((CustomListItem)Stack_CaseList.Children[0]).Margin = new Thickness();
            Instance.ItemList.Add(value);
            Instance.ItemAdded?.Invoke(value);

            Grid_Tool.Margin = new Thickness(0, 10, 0, 0);

            mousePoint.Item2 += 35;
            OSTool.SetMousePoint(mousePoint.Item1, mousePoint.Item2);
        }

        private void OnRemoveClick(CustomListItem listItem)
        {
            // 移除项
            Stack_CaseList.Children.Remove(listItem);
            if (Stack_CaseList.Children.Count > 0)
                ((CustomListItem)Stack_CaseList.Children[0]).Margin = new Thickness();
            else Grid_Tool.Margin = new Thickness();

            int index = Instance.ItemList.IndexOf(listItem.ItemValue);
            Instance.ItemList.Remove(listItem.ItemValue);
            Instance.ItemRemoved?.Invoke(index);
        }

        private void OnValueChanged(CustomListItem listItem)
        {
            int index = Stack_CaseList.Children.IndexOf(listItem);
            Instance.ItemList[index] = listItem.ItemValue;
            Instance.ItemChanged?.Invoke(index, listItem.ItemValue);
        }

        private string GenerateCaseValue()
        {
            int nameID = 1;
            while (true)
            {
                string name = $"Case_{nameID:00}";
                if (Instance.ItemList.Contains(name))
                {
                    nameID++;
                    continue;
                }
                return name;
            }
        }
    }
}