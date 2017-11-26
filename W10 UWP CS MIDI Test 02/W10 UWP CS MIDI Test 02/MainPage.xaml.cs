using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace W10_UWP_CS_MIDI_Test_02
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DeviceInformationCollection midiInputDevicesCollection;
        private DeviceInformationCollection midiOutputDevicesCollection;
        //
        private MidiEngine mdiEngine;
        //
        //
        //
        public MainPage()
        {
            this.InitializeComponent();
        }
        //
        //
        //
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            mdiEngine = new MidiEngine();
            //
            //
            //
            await PopulateMidiInputDeviceList();
            await PopulateMidiOutputDeviceList();
            //
            //
            //
            mdiEngine.MidiInputMessageReceived += new EventHandler(this.UpdateMidiInputMessageListView);
            mdiEngine.MidiOutputMessageSent += new EventHandler(this.UpdateMidiOutputMessageListView);
        }
        //
        //
        //
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }
        //
        //
        //
        private async Task PopulateMidiInputDeviceList()
        {
            MidiInputDevicesListBox.Items.Clear();
            //
            midiInputDevicesCollection = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());
            //
            foreach (var device in midiInputDevicesCollection)
            {
                MidiInputDevicesListBox.Items.Add(device.Name);
            }
            //
            MidiInputDevicesListBox.SelectedIndex = 0;
        }
        //
        //
        //
        private async Task PopulateMidiOutputDeviceList()
        {
            MidiOutputDevicesListBox.Items.Clear();
            //
            midiOutputDevicesCollection = await DeviceInformation.FindAllAsync(MidiOutPort.GetDeviceSelector());
            //
            foreach (var device in midiInputDevicesCollection)
            {
                MidiOutputDevicesListBox.Items.Add(device.Name);
            }
            //
            MidiOutputDevicesListBox.SelectedIndex = 0;
        }
        //
        //
        //
        private async void MidiInputDevicesListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceInformation di = midiInputDevicesCollection.ElementAt(MidiInputDevicesListBox.SelectedIndex);
            await mdiEngine.setMidiInputPortId(di);
        }
        //
        private async void MidiOutputDevicesListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.mdiEngine.midiOutputPort = await MidiOutPort.FromIdAsync(midiOutputDevicesCollection[MidiOutputDevicesListBox.SelectedIndex].Id);
            DeviceInformation di = midiInputDevicesCollection.ElementAt(MidiOutputDevicesListBox.SelectedIndex);
            //await mdiEngine.setMidiOutputPortId(di);
        }
        //
        //
        //
        private async void UpdateMidiInputMessageListView(object sender, EventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
            {
                MidiMessagesReceivedListView.Text += mdiEngine.midiMessagesReceived;
                MidiMessagesReceivedListView.Text += "\n";
            });
           
        }
        //
        //
        //
        private async void UpdateMidiOutputMessageListView(object sender, EventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MidiMessagesSentListView.Text += (Environment.NewLine + mdiEngine.midiMessagesSent);
            });
        }
    }
}
