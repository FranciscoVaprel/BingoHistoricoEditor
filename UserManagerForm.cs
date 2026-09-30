namespace BingoHistoricoEditor {
    /// <summary>
    /// Formulario para gestionar la lista de usuarios:
    /// ver quién existe, añadir nuevos y eliminar existentes.
    /// Solo accesible tras verificación de identidad.
    /// </summary>
    public class UserManagerForm : Form {
        private ListBox   _lstUsuarios = null!;
        private Button    _btnAnadir   = null!;
        private Button    _btnEliminar = null!;
        private Button    _btnCambiarPass = null!;
        private Button    _btnCerrar   = null!;
        private Label     _lblInfo     = null!;

        public UserManagerForm() {
            BuildUI();
            RefrescarLista();
        }

        // ── UI ────────────────────────────────────────────────────────────────

        private void BuildUI() {
            Text            = "Gestión de usuarios";
            Size            = new Size(400, 380);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false; MinimizeBox = false;
            StartPosition   = FormStartPosition.CenterParent;
            BackColor       = SystemColors.Control; ForeColor = SystemColors.ControlText;
            Font            = new Font("Segoe UI", 9f);

            var lblTitulo = new Label {
                Text      = "Usuarios con acceso",
                Font      = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = SystemColors.HotTrack,
                AutoSize  = true,
                Location  = new Point(20, 18),
            };

            _lstUsuarios = new ListBox {
                Size      = new Size(350, 150),
                Location  = new Point(20, 48),
                BackColor = SystemColors.Window,
                ForeColor = SystemColors.WindowText,
                BorderStyle = BorderStyle.FixedSingle,
                Font      = new Font("Segoe UI", 10f),
            };

            _lblInfo = new Label {
                Text      = "",
                ForeColor = SystemColors.GrayText,
                AutoSize  = true,
                Location  = new Point(20, 206),
            };

            _btnAnadir = Boton("＋  Añadir usuario",   new Point(20,  230), new Size(165, 36));
            _btnCambiarPass = Boton("🔑  Cambiar contraseña", new Point(195, 230), new Size(175, 36));
            _btnEliminar = Boton("✕  Eliminar usuario", new Point(20,  278), new Size(165, 36));
            _btnCerrar   = Boton("Cerrar",              new Point(195, 278), new Size(175, 36));

            _btnAnadir.Click      += BtnAnadir_Click;
            _btnCambiarPass.Click += BtnCambiarPass_Click;
            _btnEliminar.Click    += BtnEliminar_Click;
            _btnCerrar.Click      += (_, _) => Close();

            Controls.AddRange(new Control[] {
                lblTitulo, _lstUsuarios, _lblInfo,
                _btnAnadir, _btnCambiarPass, _btnEliminar, _btnCerrar
            });
        }

        private static Button Boton(string texto, Point loc, Size size) {
            return new Button {
                Text = texto, Location = loc, Size = size,
                ForeColor = SystemColors.ControlText,
                FlatStyle = FlatStyle.Standard, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                UseVisualStyleBackColor = true,
            };
        }

        // ── Eventos ───────────────────────────────────────────────────────────

        private void BtnAnadir_Click(object sender, EventArgs e) {
            if (!RequerirAutenticacion()) return;

            using var dlg = new SetPasswordForm(primerArranque: false);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try {
                UserStore.AgregarUsuarioConHash(dlg.UsuarioGenerado, dlg.HashGenerado, dlg.EsAdminGenerado);
                RefrescarLista();
                MostrarInfo($"Usuario '{dlg.UsuarioGenerado}' añadido correctamente.");
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCambiarPass_Click(object sender, EventArgs e) {
            string? seleccionado = _lstUsuarios.SelectedItem?.ToString();
            if (seleccionado is null) {
                MessageBox.Show("Selecciona un usuario de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }

            if (!RequerirAutenticacion()) return;

            using var dlg = new CambiarPasswordForm(seleccionado);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try {
                UserStore.CambiarPassword(seleccionado, dlg.NuevaPassword);
                MostrarInfo($"Contraseña de '{seleccionado}' actualizada.");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e) {
            string? seleccionado = _lstUsuarios.SelectedItem?.ToString();
            if (seleccionado is null) {
                MessageBox.Show("Selecciona un usuario de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }

            if (!RequerirAutenticacion()) return;

            var confirm = MessageBox.Show(
                $"¿Eliminar el usuario '{seleccionado}'?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            try {
                UserStore.EliminarUsuario(seleccionado);
                RefrescarLista();
                MostrarInfo($"Usuario '{seleccionado}' eliminado.");
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "No se puede eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Pide las credenciales de un usuario existente antes de permitir una
        /// operación sensible (añadir, eliminar o cambiar contraseña).
        /// </summary>
        private bool RequerirAutenticacion() {
            using var auth = new AutenticacionPrevia();
            if (auth.ShowDialog() == DialogResult.OK) return true;

            MessageBox.Show("Esta acción requiere autenticación.", "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void RefrescarLista() {
            _lstUsuarios.Items.Clear();
            foreach (var u in UserStore.CargarUsuarios())
                _lstUsuarios.Items.Add(u.Username);

            _btnEliminar.Enabled    = _lstUsuarios.Items.Count > 1;
            _btnCambiarPass.Enabled = _lstUsuarios.Items.Count > 0;
        }

        private void MostrarInfo(string msg) {
            _lblInfo.Text = msg;
        }
    }

    // ── Diálogo simple para cambiar contraseña ────────────────────────────────

    internal class CambiarPasswordForm : Form {
        public string NuevaPassword { get; private set; } = "";
        private TextBox _txtNueva = null!, _txtRepetir = null!;
        private Label   _lblErr   = null!;

        public CambiarPasswordForm(string usuario) {
            Text = $"Cambiar contraseña — {usuario}";
            Size = new Size(320, 260);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control; ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);

            var lbl1 = new Label { Text = "Nueva contraseña",  ForeColor = SystemColors.GrayText, AutoSize = true, Location = new Point(24, 20) };
            var lbl2 = new Label { Text = "Repetir contraseña",ForeColor = SystemColors.GrayText, AutoSize = true, Location = new Point(24, 80) };

            _txtNueva   = new TextBox { UseSystemPasswordChar = true, Size = new Size(268, 26),
                Location = new Point(24, 40), BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText,
                BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10f) };
            _txtRepetir = new TextBox { UseSystemPasswordChar = true, Size = new Size(268, 26),
                Location = new Point(24, 100), BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText,
                BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10f) };

            _lblErr = new Label { ForeColor = Color.Red, AutoSize = true,
                Location = new Point(24, 136), Visible = false };

            var btnOk = new Button { Text = "Guardar", Size = new Size(268, 36),
                Location = new Point(24, 164), ForeColor = SystemColors.ControlText,
                FlatStyle = FlatStyle.Standard, Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand, UseVisualStyleBackColor = true };
            btnOk.Click += (_, _) => Guardar();

            Controls.AddRange(new Control[] { lbl1, _txtNueva, lbl2, _txtRepetir, _lblErr, btnOk });
        }

        private void Guardar() {
            if (_txtNueva.Text.Length < 4) {
                _lblErr.Text = "Mínimo 4 caracteres."; _lblErr.Visible = true; return;
            }
            if (_txtNueva.Text != _txtRepetir.Text) {
                _lblErr.Text = "Las contraseñas no coinciden."; _lblErr.Visible = true;
                _txtRepetir.Clear(); return;
            }
            NuevaPassword = _txtNueva.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
