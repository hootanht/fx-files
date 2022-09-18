﻿using System.ComponentModel;

using Functionland.FxFiles.App.Components.Common;
using Microsoft.AspNetCore.Components.Web;

namespace Functionland.FxFiles.App.Components
{
    public partial class FxListView
    {
        [Parameter]
        public string ListTitle { get; set; } = String.Empty;

        [Parameter]
        public bool HasSearchInput { get; set; } = false;

        [Parameter, EditorRequired]
        public List<ListItemConfig>? ListItems { get; set; }

        public ViewModeEnum ViewMode = ViewModeEnum.list;
        public SortOrderEnum SortOrder = SortOrderEnum.asc;
        public bool IsSelectedAll = false;

        public void ToggleSortOrder()
        {
            if (SortOrder == SortOrderEnum.asc)
            {
                SortOrder = SortOrderEnum.desc;
            }
            else
            {
                SortOrder = SortOrderEnum.asc;
            }

            //todo: change order of list items
        }

        public void OnSortChange()
        {
            //todo: Open sort bottom sheet
        }

        public void ToggleSelectedAll()
        {
            IsSelectedAll = !IsSelectedAll;
            //todo: select all items
        }

        public void ChangeViewMode(ViewModeEnum mode)
        {
            ViewMode = mode;
        }

        public void GoIntoItem()
        {
            //todo: go into folder or open file preview if it's file
        }

        public void OpenItemOverFlow()
        {
            //todo: open folder of file overflow bottom sheet
        }
    }
}