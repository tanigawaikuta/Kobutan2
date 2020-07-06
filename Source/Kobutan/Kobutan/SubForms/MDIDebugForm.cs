using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using KobutanLib.Devices;
using KobutanLib.GameProgramming;
using KobutanLib.Communication;
using KobutanLib.Robots;
using KobutanLib.Management;
using KobutanLib.COP;

namespace Kobutan.SubForms
{
    /// <summary>
    /// デバッグ用フォーム
    /// </summary>
    public partial class MDIDebugForm : MDIBaseForm
    {
        private LayerList _LayerList;
        private LayerActivater _LayerActivater;
        private MethodDispatcher _MethodDispatcher;
        private LayerdObjectCreater _LayerdObjectCreater;

        /// <summary>
        /// デバッグ用
        /// </summary>
        public MDIDebugForm()
        {
            InitializeComponent();
            listView1.View = View.Details;
        }

        /// <summary>
        /// デバッグ用
        /// </summary>
        /// <param name="parent">親フォーム</param>
        /// <param name="kobutanSystem">こぶたんシステム。</param>
        public MDIDebugForm(Form parent, KobutanSystem kobutanSystem)
            : base(parent, kobutanSystem)
        {
            InitializeComponent();
            listView1.View = View.Details;
            //_DirectInputDeviceManager = new DirectInputDeviceManager();
            //_GRColorSensor = new GRColorSensor("192.168.201.201", 11111);

            //Type type = typeof(GRColorSensor);
            //listBox1.Items.Add(type.AssemblyQualifiedName);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _LayerList = new LayerList(new BaseLayer1(), new Layer1());
            COPManager.Instance.AddContextRegion("aho", _LayerList);

            var a = (BaseLayer1.A)COPManager.Instance["aho"].LayerdObjectCreater.CreateObject(typeof(BaseLayer1.A));
            a.M1();

            COPManager.Instance["aho"].LayerActivater.Activate("Layer1");
            a.M1();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
        }

        private void Button3_Click(object sender, EventArgs e)
        {
        }

        private void Button4_Click(object sender, EventArgs e)
        {
        }
    }

    public class BaseLayer1 : BaseLayer
    {
        public BaseLayer1()
        {
        }

        public class A
        {
            public A() { }

            public virtual void M1()
            {
                MessageBox.Show("aho");
            }

            public virtual void M2()
            {
                MessageBox.Show("aaaaa");
            }
        }
    }

    public class Layer1 : Layer
    {
        public Layer1() : base(1)
        {
        }

        public abstract class A
        {
            public A() { }

            // 自動実装
            public abstract void Proceed_M1();
            public abstract BaseLayer1.A LayerdThis { get; }

            public virtual void M1()
            {
                MessageBox.Show("baka");
                Proceed_M1();
                LayerdThis.M2();
            }
        }
    }
}
