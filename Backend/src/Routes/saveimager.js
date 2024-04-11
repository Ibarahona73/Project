import express from "express";
import { pool } from '../db.js';

const Router = express.Router();

// POST para crear una nueva imagen
Router.post("/crearimagen", async (req, res) => {
    const { rutaArchivo, description, reminderDate, tiporecordatorio, estado, id_usuario } = req.body;

    try {
        // Decodificar la imagen Base64 de la solicitud
        const imageData = Buffer.from(rutaArchivo, 'base64');

        const insertImagenQuery = `
      INSERT INTO Imagenes (RutaArchivo, description, reminderDate, CreateDate, tiporecordatorio, estado,UserId)
      VALUES (?, ?, ?, CURRENT_TIMESTAMP, ?, ?, ?)
    `;
        await pool.query(insertImagenQuery, [imageData, description, reminderDate, tiporecordatorio, estado, id_usuario]);

        res.status(200).json({ message: "Imagen creada exitosamente" });
    } catch (error) {
        console.error('Error al crear Imagen:', error.message);
        res.status(500).json({ error: 'Error al crear Imagen' });
    }
});

// GET para obtener los recordatorios con imágenes para un usuario específico
Router.get("/recordatorios-con-imagenes/:id_usuario", async (req, res) => {
    const { id_usuario } = req.params;

    try {
        const getRecordatoriosQuery = `
    SELECT RutaArchivo ,description ,reminderDate,tiporecordatorio, CreateDate,estado
    FROM Imagenes
    WHERE UserId = ?`;
        const recordatoriosConImagenes = await pool.query(getRecordatoriosQuery, [id_usuario]);

        res.status(200).json(recordatoriosConImagenes);
    } catch (error) {
        console.error('Error al obtener recordatorios con imágenes para el usuario:', error.message);
        res.status(500).json({ error: 'Error al obtener recordatorios con imágenes para el usuario' });
    }
});

export default Router;
