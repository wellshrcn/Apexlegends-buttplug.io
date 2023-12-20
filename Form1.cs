﻿using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Core;
using System.Diagnostics;
using System.Threading;
using Buttplug;
using System.Drawing;
using System.Runtime.InteropServices;

namespace 游戏玩具助手
{

    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int x, int y);

        private bool isRunning = true;


        private async Task ScanForDevices()
        {      
                await client.StartScanningAsync();
                await Task.Delay(100);
                await client.StopScanningAsync();
        }
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            label4.Text = "服务器未连接";
            label1.Text = "未启动";
            Form1_Load(null, null);
        }
        private void UpdateDeviceList()
        {
            listBox1.Items.Clear();
            foreach (var dev in client.Devices)
            {
                listBox1.Items.Add($"{dev.Index}. {dev.Name}");
            }
        }
        private ButtplugClientDevice device;
        public static ButtplugClient client;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isConnected = false;
        private async void Form1_Load(object sender, EventArgs e)
        {
            client = new ButtplugClient("游戏玩具助手");

            void HandleDeviceAdded(object aObj, DeviceAddedEventArgs aArgs)
            {
                Console.WriteLine($"Device connected: {aArgs.Device.Name}");
            }

            client.DeviceAdded += HandleDeviceAdded;

            void HandleDeviceRemoved(object aObj, DeviceRemovedEventArgs aArgs)
            {
                Console.WriteLine($"Device disconnected: {aArgs.Device.Name}");
            }

            client.DeviceRemoved += HandleDeviceRemoved;
            int retryDelay = 100;

            while (true)
            {
                try
                {
                    await client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://localhost:12345")));
                    label4.Text = "服务器已连接";
                    _isConnected = true;
                    StartProcessMonitoring();
                    break;
                }
                catch (Exception ex)
                {
                    await Task.Delay(retryDelay);
                    retryDelay *= 2;
                }
            }
        }

        private void client_Disconnect(object sender, EventArgs e)
        {
            label4.Text = "服务器未连接";
            _isConnected = false;
            _cancellationTokenSource?.Cancel();
        }

        private async void StartProcessMonitoring()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            while (_isConnected)
            {
                await Task.Delay(100);
                Process[] processes = Process.GetProcessesByName("intiface_central");
                if (processes.Length == 0)
                {
                    label4.Text = "服务器未连接";
                    _cancellationTokenSource.Cancel();
                    await client.DisconnectAsync();
                    Form1_Load(null, null);
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (label4.Text == "服务器未连接")
            {
                MessageBox.Show("请先连接到服务器！服务器为自动连接");
                return;
            }
            await ScanForDevices();
            UpdateDeviceList();
        }

        Dictionary<string, int[]> resolutionCoordinates = new Dictionary<string, int[]>()
        {
            { "1920x1080", new int[] { 200, 245, 290, 335, 385, 1000,336,981 } },
            { "2560x1440", new int[] { 260, 320, 380, 440, 500, 1335 ,447,1310} }
        };

        private async void button2_Click(object sender, EventArgs e)
        {
            if (device != null && device.VibrateAttributes.Count > 0)
            {
                label1.Text = "已启动";
                string resolutionKey = $"{Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}";
                MessageBox.Show(resolutionKey);
                if (resolutionCoordinates.TryGetValue(resolutionKey, out int[] coordinates))
                {
                    int x1 = coordinates[0];
                    int x2 = coordinates[1];
                    int x3 = coordinates[2];
                    int x4 = coordinates[3];
                    int x5 = coordinates[4];
                    int y1 = coordinates[5];
                    int x6 = coordinates[6];
                    int y2 = coordinates[7];


                    int count = 0;
                    while (isRunning)
                    {
                        Color pixelColor = GetColorAt(x6, y2);
                        if (IsColorMatch(pixelColor, ColorTranslator.FromHtml("#54534F"), 30))
                        {
                            if (IsPixelColorGray(x1, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x2, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                        }
                        else if (IsColorMatch(pixelColor, ColorTranslator.FromHtml("#3E4E79"), 30)) 
                        {
                            if (IsPixelColorGray(x1, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x2, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x3, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                        }
                        else if(IsColorMatch(pixelColor, ColorTranslator.FromHtml("#563976"), 30))
                        {
                            if (IsPixelColorGray(x1, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x2, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x3, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x4, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                        }
                        else if(IsColorMatch(pixelColor, ColorTranslator.FromHtml("#6D1E1C"), 30))
                        {
                            if (IsPixelColorGray(x1, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x2, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x3, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x4, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                            if (IsPixelColorGray(x5, y1, "#4b4b4b", 2))
                            {
                                count++;
                            }
                        }


                        await controllers(count);
                        await Task.Delay(300);

                        if (!isRunning)
                        {
                            isRunning = true;
                            label1.Text = "未启动";
                            break;
                        }

                        count = 0; 
                    }
                }
                else
                {
                    MessageBox.Show("未知分辨率");
                }
            }
            else
            {
                MessageBox.Show("该设备未连接或不支持震动");
            }
        }

        private async Task controllers(int count)
        {
            if (count == 0)
            {
                await device.VibrateAsync(0);
            }
            else if (count == 1)
            {
                await device.VibrateAsync(0.2);
            }
            else if (count == 2)
            {
                await device.VibrateAsync(0.4);
            }
            else if (count == 3)
            {
                await device.VibrateAsync(0.6);
            }
            else if (count == 4)
            {
                await device.VibrateAsync(0.8);
            }
            else if (count == 5)
            {
                await device.VibrateAsync(1);
            }
        }

        static bool IsPixelColorGray(int x, int y, string hexColor, int threshold)
        {
            Color targetColor = ColorTranslator.FromHtml(hexColor);

            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(desktopPtr, x, y);
            ReleaseDC(IntPtr.Zero, desktopPtr);

            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                                         (int)(pixel & 0x0000FF00) >> 8,
                                         (int)(pixel & 0x00FF0000) >> 16);

            return Math.Abs(color.R - targetColor.R) <= threshold &&
                   Math.Abs(color.G - targetColor.G) <= threshold &&
                   Math.Abs(color.B - targetColor.B) <= threshold;
        }
        static Color GetColorAt(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                                         (int)(pixel & 0x0000FF00) >> 8,
                                         (int)(pixel & 0x00FF0000) >> 16);

            return color;
        }
        static bool IsColorMatch(Color color1, Color color2, int threshold)
        {
            return (Math.Abs(color1.R - color2.R) <= threshold) &&
                   (Math.Abs(color1.G - color2.G) <= threshold) &&
                   (Math.Abs(color1.B - color2.B) <= threshold);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                var selectedIndex = listBox1.SelectedIndex;
                var deviceChoice = client.Devices[selectedIndex];
                device = client.Devices.FirstOrDefault(dev => dev.Index == deviceChoice.Index);
                MessageBox.Show("连接成功");
                label3.Text = "已连接";
            }
            else
            {
                MessageBox.Show("无法获取设备名称，请开启管理员模式重试");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            if (device != null && device.VibrateAttributes.Count > 0)
            {
                Console.WriteLine($"{device.Name} - Number of Vibrators: {device.VibrateAttributes.Count}");
                await device.VibrateAsync(0.1);
                await Task.Delay(1000);
                await device.VibrateAsync(0);
            }
            else 
            {
                MessageBox.Show("该设备不支持震动");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private async void button3_Click_2(object sender, EventArgs e)
        {
            isRunning = false;
            if (label4.Text == "服务器未连接")
            {
                MessageBox.Show("请先连接到服务器！服务器为自动连接");
                return;
            }
            try
            {
                await device.VibrateAsync(0.0);
            }
            catch
            {
            }

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private async void batteryLevel()
        {
            if (label4.Text == "服务器未连接")
            {
                MessageBox.Show("请先连接到服务器！服务器为自动连接");
                return;
            }
            if (device == null)
            {
                MessageBox.Show("请先连接玩具");
                return;
            }
            try
            {
                if (device.HasBattery)
                {
                    double batteryLevel = await device.BatteryAsync();
                    label6.Text = batteryLevel.ToString("0.##") + "%";
                }
                else
                {
                    label6.Text = "设备不支持电量查询";
                }
            }
            catch (ButtplugDeviceException ex2)
            {
                label6.Text = "无法查询电量: " + ex2.Message;
            }
            catch (Exception ex)
            {
                label6.Text = "查询电量时出现错误: " + ex.Message;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (label4.Text == "服务器未连接")
            {
                MessageBox.Show("请先连接到服务器！服务器为自动连接");
                return;
            }
            List<string> features = new List<string>();
            if (device.VibrateAttributes.Count > 0)
            {
                features.Add("震动");
            }
            if (device.RotateAttributes.Count > 0)
            {
                features.Add("旋转");
            }
            if (device.OscillateAttributes.Count > 0)
            {
                features.Add("振荡");
            }
            if (device.LinearAttributes.Count > 0)
            {
                features.Add("线性运动");
            }
            string featureDescriptions = string.Join(", ", features);
            if (features.Any())
            {
                MessageBox.Show(device.Name + " 支持的功能: " + featureDescriptions);
            }
            else
            {
                MessageBox.Show(device.Name + " 没有检测到支持的功能");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            batteryLevel();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
