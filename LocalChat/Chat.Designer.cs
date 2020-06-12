namespace LocalChat
{
    partial class Chat
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.countAddFileLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.шифрованиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заполнениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.padding1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.padding2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.padding3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.padding4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.режимРаботыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.длиннаКлючаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьВсеСообщенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сервисToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.информацияОПоследнейЗагрузкеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Контакты";
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(-4, 556);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 54);
            this.button1.TabIndex = 2;
            this.button1.Text = "Общие";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(142, 556);
            this.textBox1.MaxLength = 255;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(514, 52);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(142, 25);
            this.panel1.MinimumSize = new System.Drawing.Size(580, 520);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 531);
            this.panel1.TabIndex = 6;
            this.panel1.Tag = "";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(-1, 54);
            this.panel2.MinimumSize = new System.Drawing.Size(140, 400);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(148, 505);
            this.panel2.TabIndex = 0;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(725, -2);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(400, 610);
            this.textBox3.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // countAddFileLabel
            // 
            this.countAddFileLabel.AutoSize = true;
            this.countAddFileLabel.Location = new System.Drawing.Point(656, 585);
            this.countAddFileLabel.Name = "countAddFileLabel";
            this.countAddFileLabel.Size = new System.Drawing.Size(0, 13);
            this.countAddFileLabel.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.menuStrip1);
            this.panel3.Location = new System.Drawing.Point(142, -2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(581, 21);
            this.panel3.TabIndex = 12;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.шифрованиеToolStripMenuItem,
            this.данныеToolStripMenuItem,
            this.сервисToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(581, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // шифрованиеToolStripMenuItem
            // 
            this.шифрованиеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заполнениеToolStripMenuItem,
            this.режимРаботыToolStripMenuItem,
            this.длиннаКлючаToolStripMenuItem});
            this.шифрованиеToolStripMenuItem.Name = "шифрованиеToolStripMenuItem";
            this.шифрованиеToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.шифрованиеToolStripMenuItem.Text = "Шифрование";
            // 
            // заполнениеToolStripMenuItem
            // 
            this.заполнениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.padding1ToolStripMenuItem,
            this.padding2ToolStripMenuItem,
            this.padding3ToolStripMenuItem,
            this.padding4ToolStripMenuItem});
            this.заполнениеToolStripMenuItem.Name = "заполнениеToolStripMenuItem";
            this.заполнениеToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.заполнениеToolStripMenuItem.Text = "Заполнение последненго блока ";
            // 
            // padding1ToolStripMenuItem
            // 
            this.padding1ToolStripMenuItem.CheckOnClick = true;
            this.padding1ToolStripMenuItem.Name = "padding1ToolStripMenuItem";
            this.padding1ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.padding1ToolStripMenuItem.Text = "ANSIX923";
            this.padding1ToolStripMenuItem.Click += new System.EventHandler(this.padding1ToolStripMenuItem_Click);
            // 
            // padding2ToolStripMenuItem
            // 
            this.padding2ToolStripMenuItem.CheckOnClick = true;
            this.padding2ToolStripMenuItem.Name = "padding2ToolStripMenuItem";
            this.padding2ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.padding2ToolStripMenuItem.Text = "ISO10126";
            this.padding2ToolStripMenuItem.Click += new System.EventHandler(this.padding2ToolStripMenuItem_Click);
            // 
            // padding3ToolStripMenuItem
            // 
            this.padding3ToolStripMenuItem.CheckOnClick = true;
            this.padding3ToolStripMenuItem.Name = "padding3ToolStripMenuItem";
            this.padding3ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.padding3ToolStripMenuItem.Text = "PKCS7";
            this.padding3ToolStripMenuItem.Click += new System.EventHandler(this.padding3ToolStripMenuItem_Click);
            // 
            // padding4ToolStripMenuItem
            // 
            this.padding4ToolStripMenuItem.Checked = true;
            this.padding4ToolStripMenuItem.CheckOnClick = true;
            this.padding4ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.padding4ToolStripMenuItem.Name = "padding4ToolStripMenuItem";
            this.padding4ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.padding4ToolStripMenuItem.Text = "Zeros";
            this.padding4ToolStripMenuItem.Click += new System.EventHandler(this.padding4ToolStripMenuItem_Click);
            // 
            // режимРаботыToolStripMenuItem
            // 
            this.режимРаботыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Mode1ToolStripMenuItem,
            this.Mode2ToolStripMenuItem,
            this.Mode3ToolStripMenuItem,
            this.Mode4ToolStripMenuItem,
            this.Mode5ToolStripMenuItem});
            this.режимРаботыToolStripMenuItem.Name = "режимРаботыToolStripMenuItem";
            this.режимРаботыToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.режимРаботыToolStripMenuItem.Text = "Режим работы";
            // 
            // Mode1ToolStripMenuItem
            // 
            this.Mode1ToolStripMenuItem.Checked = true;
            this.Mode1ToolStripMenuItem.CheckOnClick = true;
            this.Mode1ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Mode1ToolStripMenuItem.Name = "Mode1ToolStripMenuItem";
            this.Mode1ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.Mode1ToolStripMenuItem.Text = "Сцепление блоков";
            this.Mode1ToolStripMenuItem.Click += new System.EventHandler(this.Mode1ToolStripMenuItem_Click);
            // 
            // Mode2ToolStripMenuItem
            // 
            this.Mode2ToolStripMenuItem.CheckOnClick = true;
            this.Mode2ToolStripMenuItem.Name = "Mode2ToolStripMenuItem";
            this.Mode2ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.Mode2ToolStripMenuItem.Text = "Обратной связи по шифру";
            this.Mode2ToolStripMenuItem.Click += new System.EventHandler(this.Mode2ToolStripMenuItem_Click);
            // 
            // Mode3ToolStripMenuItem
            // 
            this.Mode3ToolStripMenuItem.CheckOnClick = true;
            this.Mode3ToolStripMenuItem.Name = "Mode3ToolStripMenuItem";
            this.Mode3ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.Mode3ToolStripMenuItem.Text = "Захват зашифрованого текста";
            this.Mode3ToolStripMenuItem.Click += new System.EventHandler(this.Mode3ToolStripMenuItem_Click);
            // 
            // Mode4ToolStripMenuItem
            // 
            this.Mode4ToolStripMenuItem.CheckOnClick = true;
            this.Mode4ToolStripMenuItem.Name = "Mode4ToolStripMenuItem";
            this.Mode4ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.Mode4ToolStripMenuItem.Text = "Электронной кодовой куниги";
            this.Mode4ToolStripMenuItem.Click += new System.EventHandler(this.Mode4ToolStripMenuItem_Click);
            // 
            // Mode5ToolStripMenuItem
            // 
            this.Mode5ToolStripMenuItem.CheckOnClick = true;
            this.Mode5ToolStripMenuItem.Name = "Mode5ToolStripMenuItem";
            this.Mode5ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.Mode5ToolStripMenuItem.Text = "Выходной обратной связи";
            this.Mode5ToolStripMenuItem.Click += new System.EventHandler(this.Mode5ToolStripMenuItem_Click);
            // 
            // длиннаКлючаToolStripMenuItem
            // 
            this.длиннаКлючаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.длиннаКлючаToolStripMenuItem.Name = "длиннаКлючаToolStripMenuItem";
            this.длиннаКлючаToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.длиннаКлючаToolStripMenuItem.Text = "Длинна ключа";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.CheckOnClick = true;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(92, 22);
            this.toolStripMenuItem2.Text = "128";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Checked = true;
            this.toolStripMenuItem3.CheckOnClick = true;
            this.toolStripMenuItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(92, 22);
            this.toolStripMenuItem3.Text = "192";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.CheckOnClick = true;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(92, 22);
            this.toolStripMenuItem4.Text = "256";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалитьВсеСообщенияToolStripMenuItem,
            this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem,
            this.openDownloadToolStripMenuItem});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // удалитьВсеСообщенияToolStripMenuItem
            // 
            this.удалитьВсеСообщенияToolStripMenuItem.Name = "удалитьВсеСообщенияToolStripMenuItem";
            this.удалитьВсеСообщенияToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.удалитьВсеСообщенияToolStripMenuItem.Text = "Удалить все сообщения";
            this.удалитьВсеСообщенияToolStripMenuItem.Click += new System.EventHandler(this.удалитьВсеСообщенияToolStripMenuItem_Click);
            // 
            // удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem
            // 
            this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem.Name = "удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem";
            this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem.Text = "Удалить все загрузки и выгрузки";
            this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem.Click += new System.EventHandler(this.удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem_Click);
            // 
            // openDownloadToolStripMenuItem
            // 
            this.openDownloadToolStripMenuItem.Name = "openDownloadToolStripMenuItem";
            this.openDownloadToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.openDownloadToolStripMenuItem.Text = "Открыть папку загрузок";
            this.openDownloadToolStripMenuItem.Click += new System.EventHandler(this.открытьПапкуЗагрузокToolStripMenuItem_Click);
            // 
            // сервисToolStripMenuItem
            // 
            this.сервисToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem,
            this.информацияОПоследнейЗагрузкеToolStripMenuItem,
            this.consolToolStripMenuItem});
            this.сервисToolStripMenuItem.Name = "сервисToolStripMenuItem";
            this.сервисToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.сервисToolStripMenuItem.Text = "Сервис";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // информацияОПоследнейЗагрузкеToolStripMenuItem
            // 
            this.информацияОПоследнейЗагрузкеToolStripMenuItem.Name = "информацияОПоследнейЗагрузкеToolStripMenuItem";
            this.информацияОПоследнейЗагрузкеToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.информацияОПоследнейЗагрузкеToolStripMenuItem.Text = "Информация о последней загрузке";
            this.информацияОПоследнейЗагрузкеToolStripMenuItem.Click += new System.EventHandler(this.информацияОПоследнейЗагрузкеToolStripMenuItem_Click);
            // 
            // consolToolStripMenuItem
            // 
            this.consolToolStripMenuItem.CheckOnClick = true;
            this.consolToolStripMenuItem.Name = "consolToolStripMenuItem";
            this.consolToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.consolToolStripMenuItem.Text = "consol";
            this.consolToolStripMenuItem.Click += new System.EventHandler(this.consolToolStripMenuItem_Click);
            // 
            // loadBox1
            // 
            this.loadBox1.BackColor = System.Drawing.SystemColors.Window;
            this.loadBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loadBox1.Location = new System.Drawing.Point(659, 556);
            this.loadBox1.Name = "loadBox1";
            this.loadBox1.Size = new System.Drawing.Size(30, 30);
            this.loadBox1.TabIndex = 10;
            this.loadBox1.TabStop = false;
            this.loadBox1.Visible = false;
            this.loadBox1.Click += new System.EventHandler(this.loadBox1_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::LocalChat.Properties.Resources.scale;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(686, 554);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(37, 54);
            this.button3.TabIndex = 5;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::LocalChat.Properties.Resources.paperClip;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(652, 554);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 54);
            this.button2.TabIndex = 4;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ClientSize = new System.Drawing.Size(721, 607);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.countAddFileLabel);
            this.Controls.Add(this.loadBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Chat";
            this.Text = "Local Chat by LepseyName";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Chat_FormClosed);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox loadBox1;
        private System.Windows.Forms.Label countAddFileLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem шифрованиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сервисToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem информацияОПоследнейЗагрузкеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заполнениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem режимРаботыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem длиннаКлючаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem Mode1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Mode2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Mode3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Mode4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Mode5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem padding1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem padding2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem padding3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem padding4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьВсеСообщенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consolToolStripMenuItem;
    }
}