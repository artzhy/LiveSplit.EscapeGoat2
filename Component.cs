﻿using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.EscapeGoat2Autosplitter.State;
using LiveSplit.EscapeGoat2Autosplitter.Memory;

namespace LiveSplit.EscapeGoat2Autosplitter
{
    public class EscapeGoat2Component : LogicComponent
    {
        //public EscapeGoat2Settings Settings { get; set; }

        public override string ComponentName {
            get { return "Escape Goat 2 Auto Splitter"; }
        }

        public GoatState goatState;

        protected TimerModel Model { get; set; }

        public EscapeGoat2Component() {
            goatState = new GoatState();
            goatState.goatTriggers.OnSplit += OnSplit;
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) {
            if (Model == null) {
                Model = new TimerModel() { CurrentState = state };
                state.OnReset += OnReset;
                state.OnPause += OnPause;
                state.OnResume += OnResume;
                state.OnStart += OnStart;
                state.OnSkipSplit += OnSkipSplit;
            }

            goatState.Loop();
            goatState.goatTriggers.timerRunning = (Model.CurrentState.CurrentPhase == TimerPhase.Running);
        }

        public void OnSkipSplit(object sender, EventArgs e) {
            write("[LiveSplit] Skip Split.");
            //goatState.goatTriggers.GoToNextSplit();
        }

        public void OnResume(object sender, EventArgs e) {
            write("[LiveSplit] Resume.");
            goatState.goatTriggers.timerRunning = (Model.CurrentState.CurrentPhase == TimerPhase.Running);
        }

        public void OnPause(object sender, EventArgs e) {
            write("[LiveSplit] Pause.");
            goatState.goatTriggers.timerRunning = (Model.CurrentState.CurrentPhase == TimerPhase.Running);
        }

        public void OnStart(object sender, EventArgs e) {
            write("[LiveSplit] Start.");
            goatState.goatTriggers.timerRunning = (Model.CurrentState.CurrentPhase == TimerPhase.Running);
        }

        public void OnReset(object sender, TimerPhase e) {
            write("[LiveSplit] Reset.");
            goatState.Reset();
            goatState.goatTriggers.timerRunning = (Model.CurrentState.CurrentPhase == TimerPhase.Running);
        }

        public void OnSplit(object sender, SplitEventArgs e) {
            if (e.name == "Start") {
                write("[OriSplitter] Start.");
                Model.Start();
            } else {
                write("[OriSplitter] Split.");
                Model.Split();
            }
        }

        public override void Dispose() {
        }

        public override Control GetSettingsControl(LayoutMode mode) {
            return null;
        }

        public override void SetSettings(XmlNode settings) {
        }

        public override XmlNode GetSettings(XmlDocument document) {
            return document.CreateElement("x");
        }

        private void write(string str) {
            #if DEBUG
            StreamWriter wr = new StreamWriter("_goatauto.log", true);
            wr.WriteLine("[" + DateTime.Now + "] " + str);
            wr.Close();
            #endif
        }
    }
}