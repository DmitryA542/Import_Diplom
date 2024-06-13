using DevExpress.Spreadsheet;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masterconfig
{
    class SecondStageOperationsClass
    {
        private List<string> _ListOfColumnExcel = new List<string>();

        private bool _AutoSheetFlag = false;
        private string _SheetName = "";
        private bool _AutoTableFlag = false;
        private int _ColumnIndex = 0;
        private int _RowIndex = 0;

        private int _x = 0, _y = 0;

        public void SetAutoSheetFlag(bool autoSheetFlag)
        {
            _AutoSheetFlag = autoSheetFlag;
        }

        public bool GetAutoSheetFlag()
        {
            return _AutoSheetFlag;
        }

        public void SetSheetName(string sheetName)
        {
            _SheetName = sheetName;
        }

        public string GetSheetName()
        {
            return _SheetName;
        }

        public void SetAutoTableFlag(bool autoTableFlag)
        {
            _AutoTableFlag = autoTableFlag;
        }

        public bool GetAutoTableFlag()
        {
            return _AutoTableFlag;
        }

        public void SetColumnIndex(int columnIndex)
        {
            _ColumnIndex = columnIndex;
        }

        public int GetColumnIndex()
        {
            return _ColumnIndex;
        }

        public void SetRowIndex(int rowIndex)
        {
            _RowIndex = rowIndex;
        }

        public int GetRowIndex()
        {
            return _RowIndex;
        }

        public List<string> GetList()
        {
            return _ListOfColumnExcel;
        }

        public void SetList()
        {
            _ListOfColumnExcel = new List<string>();
        }

        public (bool, CellRange) GetFlagIsTableExists(Worksheet worksheet, int start_x, int start_y)
        {
            int x = start_x; int y = start_y;
            var FlagTableExists = false;
            var cell = worksheet.Cells[x, y];
            while (y < 20)
            {
                while (x < 20)
                {
                    if (IsCellValueNotEmpty(cell) ||
                        IsCellHasBorders(cell) ||
                        IsCellHasBackground(cell))
                    {
                        FlagTableExists = true;
                        break;
                    }
                    cell = worksheet.Cells[x, y];
                    x++;
                }
                if (FlagTableExists)
                {
                    break;
                }
                x = 0;
                y++;
            }
            if (x != 0)
            {
                x--;
            }
            _x = x;
            _y = y;
            if (CheckTable(worksheet, x, y).Item1)
            {
                _SheetName = worksheet.Name;
                return (true, CheckTable(worksheet, x, y).Item2);
            }
            else
            {
                return GetFlagIsTableExists(worksheet, x + 1, y);
            }
        }

        public (bool, CellRange) CheckTable(Worksheet worksheet, int x, int y)
        {
            int FinalPosition_x = x, FinalPosition_y = y;
            int CountColumns = 0; int CountRows = 0;
            int MaximumHeight = -1;
            var cell = worksheet.Cells[x, y];
            while (IsCellValueNotEmpty(cell) ||
                    IsCellHasBorders(cell) ||
                    IsCellHasBackground(cell))
            {
                while (IsCellValueNotEmpty(cell) ||
                    IsCellHasBorders(cell) ||
                    IsCellHasBackground(cell))
                {
                    FinalPosition_x += 1;
                    cell = worksheet.Cells[FinalPosition_x, FinalPosition_y];
                }
                if (FinalPosition_x - 1 > MaximumHeight)
                {
                    MaximumHeight = FinalPosition_x - 1;
                }
                FinalPosition_x = x;
                FinalPosition_y += 1;
                CountColumns++;
                cell = worksheet.Cells[FinalPosition_x, FinalPosition_y];
            }
            CountRows += MaximumHeight - x + 1;
            if (CountRows >= 2 && CountColumns >= 2)
            {
                return (true, worksheet.Range.FromLTRB(y, x, FinalPosition_y - 1, MaximumHeight));
            }
            else
            {
                return (false, null);
            }
        }

        public void AutoFillTable(Worksheet worksheet, bool FlagAutoSearch, int StartPosition_x = 0, int StartPosition_y = 0)
        {
            _ListOfColumnExcel.Clear();
            var x = StartPosition_y; var y = StartPosition_x;
            var FinalPosition_x = 0; var FinalPosition_y = 0;
            var FlagFoundedCell = false;
            var cell = worksheet.Cells[StartPosition_y, StartPosition_x];
            if (FlagAutoSearch)
            {
                x = _x; y = _y;
                while (y < 20)
                {
                    while (x < 20)
                    {
                        if (IsCellValueNotEmpty(cell) ||
                            IsCellHasBorders(cell) ||
                            IsCellHasBackground(cell))
                        {
                            FlagFoundedCell = true;
                            break;
                        }
                        cell = worksheet.Cells[x, y];
                        FinalPosition_x = x;
                        FinalPosition_y = y;
                        x++;
                    }
                    if (FlagFoundedCell)
                    {
                        break;
                    }
                    x = StartPosition_x;
                    y++;
                }
                x--;
                _ColumnIndex = FinalPosition_y;
                _RowIndex = FinalPosition_x;
                while (IsCellValueNotEmpty(cell) ||
                    IsCellHasBorders(cell) ||
                    IsCellHasBackground(cell))
                {
                    FinalPosition_y += 1;
                    _ListOfColumnExcel.Add(cell.Value.TextValue);
                    cell = worksheet.Cells[FinalPosition_x, FinalPosition_y];
                }
            }
            else
            {
                FinalPosition_x = StartPosition_y; FinalPosition_y = StartPosition_x;
                cell = worksheet.Cells[FinalPosition_x, FinalPosition_y];
                while (IsCellValueNotEmpty(cell) ||
                    IsCellHasBorders(cell) ||
                    IsCellHasBackground(cell))
                {
                    FinalPosition_y += 1;
                    _ListOfColumnExcel.Add(cell.Value.TextValue);
                    cell = worksheet.Cells[FinalPosition_x, FinalPosition_y];
                }
            }
        }

        private bool IsCellValueNotEmpty(Cell cell)
        {
            return !cell.Value.IsEmpty;
        }

        private bool IsCellHasBorders(Cell cell)
        {
            return cell.Borders.TopBorder.LineStyle.ToString() != "None" ||
                   cell.Borders.RightBorder.LineStyle.ToString() != "None" ||
                   cell.Borders.LeftBorder.LineStyle.ToString() != "None" ||
                   cell.Borders.BottomBorder.LineStyle.ToString() != "None";
        }

        private bool IsCellHasBackground(Cell cell)
        {
            return !cell.Fill.BackgroundColor.IsEmpty;
        }
    }
}
