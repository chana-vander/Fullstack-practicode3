// const express = require('express');
// const axios = require('axios');
// require('dotenv').config();

// const app = express();
// const port = process.env.PORT || 3000;

// const RENDER_API_KEY = process.env.RENDER_API_KEY;

// // Endpoint של GET לקבלת רשימת האפליקציות
// app.get('/apps', async (req, res) => {
//   try {
//     // קריאה ל-API של Render עם ה-API Key
//     const response = await axios.get('https://authserver6.onrender.com', {
//       headers: {
//         Authorization: `Bearer ${RENDER_API_KEY}`,
//       },
//     });
//     res.json(response.data);  // מחזירים את הנתונים כ-JSON
//   } catch (error) {
//     console.error(error);
//     res.status(500).send('שגיאה בקריאה ל-Render API');
//   }
// });

// // התחלת השרת
// app.listen(port, () => {
//   console.log(`השרת רץ על פורט ${port}`);
// });
require("dotenv").config();
const express = require("express");
const axios = require("axios");

const app = express();
const PORT = process.env.PORT || 3000;

// API של Render
const RENDER_API_URL = "https://api.render.com/v1/services";
const API_KEY = "rnd_0iEhIPxHEdh1HGb6e3BgDqa7vktd";

// נקודת קצה שמחזירה את רשימת האפליקציות
app.get("/", async (req, res) =>{
  try {
    const response = await axios.get(RENDER_API_URL, {
      headers: {
        Authorization: `Bearer ${API_KEY}`,
      },
    });

    res.json(response.data);
  } catch (error) {
    console.error("Error fetching data from Render API:", error);
    res.status(500).json({ error: "Failed to fetch data from Render API" });
  }
});

// הפעלת השרת
app.listen(PORT, () => {
  console.log(`🚀 Server running on http://localhost:${PORT}`);
});
