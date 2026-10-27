# Verificación de la Base de Datos y Tabla de Operaciones

## 📁 Estructura de Archivos

Tu base de datos Firebird debe estar en:
```
C:\CSHARP\BingoHistoricoEditor\BBDD\AAMSLOG.FDB
```

Según `appsettings.json`:
- **DbRelativePath**: `BBDD/AAMSLOG.FDB`
- **DbModo**: `Local`

## ✅ ¿Qué significa cada estado?

### 1. **Archivo .fdb NO existe**
```
BD configurada en: C:\...\BBDD\AAMSLOG.FDB
ADVERTENCIA: El archivo .fdb NO existe. Se creará al conectar.
```
✓ Es normal la **primera vez**. Firebird lo creará automáticamente.

### 2. **Conexión exitosa**
```
✓ Archivo .fdb encontrado.
Conexión establecida correctamente.
✓ Tabla 'operacionesBGEditor' creada.
✓ Generador 'GEN_OPERACIONESBGEDITOR_ID' creado.
✓ Trigger 'BI_OPERACIONESBGEDITOR_ID' creado.
```
✓ **TODO FUNCIONA CORRECTAMENTE**

### 3. **Error de conexión**
Si ves: `Estado: ERROR DE CONEXIÓN`

**Posibles causas:**
- Firebird 2.5 no está instalado
- Firebird no está ejecutándose como servicio
- Las credenciales locales de Firebird son incorrectas
- El puerto 3050 está bloqueado
- Problema de permisos en la carpeta BBDD

## 🔍 Cómo Verificar

### Opción 1: Con PowerShell (Script)
```powershell
cd C:\CSHARP\BingoHistoricoEditor
.\VerificarBD.ps1
```

### Opción 2: Manualmente
1. Abre el Explorador
2. Ve a: `C:\CSHARP\BingoHistoricoEditor\BBDD\`
3. Verifica que exista `AAMSLOG.FDB`
4. Si no existe, ejecuta la aplicación una vez (la creará)

### Opción 3: Con IBExpert (Firebird GUI)
1. Abre IBExpert
2. Conecta a: `localhost:C:\CSHARP\BingoHistoricoEditor\BBDD\AAMSLOG.FDB`
3. Usuario configurado para la conexión
4. Contraseña configurada localmente (no compartirla)
5. En "Tables" deberías ver:
   - `HISTORICO` (tabla de partidas)
   - `OPERACIONESBGEDITOR` (tabla de operaciones)

### Opción 4: SQL en IBExpert
```sql
-- Ver todas las tablas
SELECT RDB$RELATION_NAME 
FROM RDB$RELATIONS 
WHERE RDB$SYSTEM_FLAG = 0 
ORDER BY RDB$RELATION_NAME;

-- Ver operaciones registradas
SELECT * FROM operacionesBGEditor ORDER BY ID DESC;

-- Contar operaciones
SELECT COUNT(*) AS total FROM operacionesBGEditor;
```

## 🛠️ Solución de Problemas

### "Archivo .fdb NO existe"
→ Ejecuta la aplicación una vez. Se creará automáticamente.

### "Connection refused"
→ Inicia Firebird: `net start FirebirdServerDefaultInstance`

### "Usuario/Contraseña incorrecta"
→ Verifica en `appsettings.json`:
```json
"DbUser": "SYSDBA",
"DbPass": "<contraseña local; no publicar>"
```

### "Tabla no se crea"
→ Revisa los logs en `C:\CSHARP\BingoHistoricoEditor\logs\operaciones.json`

## 📊 Información del Servicio de Firebird

Versión requerida: **Firebird 2.5**

Para verificar que está instalado:
```powershell
Get-Service | Where-Object {$_.Name -like "*Firebird*"}
```

Debería mostrar algo como:
```
Status   Name
------   ----
Running  FirebirdServerDefaultInstance
```
