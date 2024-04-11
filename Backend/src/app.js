import express from 'express';
import bodyParser from 'body-parser';
import { pool } from './db.js';
import { PORT } from './config.js';
import resetPassword from './Routes/resetPassword.js';
import newpassword from './Routes/newpassword.js';
import verificationcod from './Routes/verificationcod.js';
import savereminders from './Routes/savereminders.js';
import savenotaaudio from './Routes/savenotaudio.js';
import saveimager from './Routes/saveimager.js';

const app = express();
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// Asigna las rutas
app.use('/sendcod', resetPassword);
app.use('/verificationcode', verificationcod);
app.use('/newpassword', newpassword);
app.use('/',savereminders);
app.use('/',savenotaaudio);
app.use('/',saveimager);
app.get('/', async (req, res) => {
    const [rows] = await pool.query('SELECT * FROM Usuarios');
    res.json(rows);
});

app.post('/login', async (req, res) => {
    const { Email, Password } = req.body;

    // Verificar si el usuario existe en la base de datos
    const [rows] = await pool.query('SELECT * FROM Usuarios WHERE Email = ? AND Password = ?', [Email, Password]);

    if (rows.length === 1) {
        // Usuario autenticado
        res.json({ message: 'Login exitoso', usuario: rows[0] });
    } else {
        // Credenciales inválidas
        res.status(401).json({ error: 'Credenciales inválidas' });
    }
});

app.get('/recordatorio', async (req, res) => {
    const { userId } = req.query;

    if (!userId) {
        return res.status(400).json({ error: 'Se requiere el ID de usuario' });
    }

    try {
        const [rows] = await pool.query('SELECT * FROM Tareas WHERE usuario_id = ?', [userId]);
        res.json(rows);
    } catch (error) {
        console.error('Error al obtener las tareas del usuario:', error.message);
        res.status(500).json({ error: 'Error al obtener las tareas del usuario' });
    }
});

app.get('/ping', async (req, res) => {
    const [Result] = await pool.query('SELECT "hello world " as RESULT');
    console.log(Result);
    res.json(Result[0]);
});

app.post('/createuser', async (req, res) => {
    const Result = await pool.query('INSERT INTO Usuarios(NombreUsuario,Password,NombreCompleto,Email) VALUES ("?","?","?","?")');
    res.json(Result);
});

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
