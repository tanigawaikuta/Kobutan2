using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kobutan.SubForms
{
    /// <summary>
    /// 設定変更のためのフォーム。
    /// </summary>
    public partial class OptionForm : Form
    {
        /// <summary>
        /// 設定変更のためのフォーム。
        /// </summary>
        public OptionForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
            // ツリービューの展開
            _TreeView.ExpandAll();
        }
    }
}
