
using Animatrix.Animation;
using Animatrix.Projectors;
using Animatrix.Screener;
using Animatrix.Screener.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Animatrix
{
    public enum Animtion
    {
        Debug =0, // For debugin purpose.
        SlideIn_FromLeft=1,
        SlideIn_FromRight=2,
        SlideIn_FromTop=3,
        SlideIn_FromBottom=4,
        SlideOut_ToLeft=5,
        SlideOut_ToRight=6,
        SlideOut_ToBottom=7,
        SlideOut_ToTop=8,
        Rotate_FromCenter=9
    }
  public   class Animator :Component
    {
        private System.Windows.Forms.Timer timer1;
        private IContainer components;
        List<IProjector> lst;

        public  bool Started { get { return timer1.Enabled; } }

        public Animator()
        {
            this.InitializeComponent();
            lst = new List<IProjector>();
            
        }

        public void Start()
        {
            this.timer1.Start();
        }
        public void Pause()
        {
            this.timer1.Stop();
        }
      
        private int _fps = 100;
        public int FPS
        {
            get {
                return _fps; 
            }
            set {
                timer1.Interval =  1000/value ;
                _fps = value;
            }
        }

        public void Add(IProjector pj)
        {
            lock (lst)
            {
                bool exits = false;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].Equals(pj)) exits = true; break; ;
                }
               if(!exits) lst.Add(pj);
            }
        }
        public ManualResetEvent Add(Control ctrl, IAnimation an)
        {
            if (this.hostExists(ctrl)) return new ManualResetEvent(true);
            ControlScreener sc = new ControlScreener(ctrl);
            Projectors.Projector  pj = new Projectors.Projector ();
            var wait= pj.Inti(sc, an, true);
            this.Add(pj);
            return wait;
        }
        public ManualResetEvent Add(Control ctrl, IAnimation an, Boolean Show)
        {
            if (this.hostExists(ctrl)) return new ManualResetEvent(true);
            ControlScreener sc = new ControlScreener(ctrl);
            Projectors.Projector pj = new Projectors.Projector();
            var wait = pj.Inti(sc, an, Show );
            this.Add(pj);
            return wait;
        }

        public void AddReflection(Control cntrl)
        {
            if (this.hostExists(cntrl)) return;
            Projector pj = new Projector();
            ReflectionOptmized rf = new ReflectionOptmized(cntrl.Parent.BackColor);
           // rf.Position = Direction.Top;
            pj.Inti(new Reflector(cntrl),rf, true);
            this.Add(pj);            
        }

        public void AddReflection(Control cntrl,Direction dir)
        {
            if (this.hostExists(cntrl)) return;
            Projector pj = new Projector();
            ReflectionOptmized rf = new ReflectionOptmized(cntrl.Parent.BackColor);
            rf.Position = dir ;
            pj.Inti(new Reflector(cntrl), rf, true);
            this.Add(pj);
        }

        private bool  hostExists(Object host)
        {
            bool exits = false;
            lock (lst)
            {                
                for (int i = 0; i < lst.Count; i++)
                {
                   if (lst[i].getHost().Equals(host)) {exits =true ; break;}
                }                
            }
            return exits;
        }

        public void RemoveAll()
        {
            lock (lst)
            {
                for (int i = 0; i < lst.Count; i++)
                {                   
                        lst[i].CleanMemoryFootprints();                   
                }
            }
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try { removeCompleted(); }
            catch (Exception) { }

            try { moveNext(); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            
        }

        private void removeCompleted()
        {
            lock (lst)
            {
                for (int i = 0; i < lst.Count ; i++)
                {
                    if (lst[i].Completed)
                    {
                        lst[i].CleanMemoryFootprints();
                        lst.RemoveAt(i);
                    }
                }
            }
        }

        private void moveNext()
        {
            lock (lst)
            {
                for (int i = 0; i < lst.Count ; i++)
                {
                    if (!lst[i].Completed)
                    {
                        lst[i].NextFrame();
                    }
                }
            }
        }

    }
}
