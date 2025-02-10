// import axios from 'axios';

// axios.defaults.baseURL = process.env.REACT_APP_API_URL||"http://localhost:5000"

// //טיפול בשגיאות:
// axios.interceptors.response.use(
//   //אם אין שגיאה
//   (response)=>{
//     return response
//   },
//   //אם יש שגיאה
//   (error)=>{
//     console.error("error in Api: ",error.response?.status,error.message);
//     return Promise.reject(error);
//   }  
// );

// export default {
//   getTasks: async () => {
//     const result = await axios.get(`/items`)    
//     return result.data;
//   },

//   addTask: async(name)=>{
//     console.log('addTask', name)
//     const result=await axios.post(`/items`,{name,isComplete:false});
//     return result.data;
//   },

//   setCompleted: async(id, isComplete)=>{
//     console.log('setCompleted', {id, isComplete})
//     const result=await axios.put(`/items/${id}`,{isComplete});
//     return result.data;
//   },

//   deleteTask:async(id)=>{
//     console.log('deleteTask');
//     const result=await axios.delete(`/items/${id}`);
//     return result.data;
//   }
// };
import axios from 'axios';

// const apiUrl = Api.env.REACT_APP_API_URL || "http://localhost:5030";
const apiUrl = process.env.REACT_APP_API_URL || "https://authserver6.onrender.com/";


// יצירת מופע axios עם baseURL
const api = axios.create({
  baseURL: apiUrl,
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor ללכידת שגיאות
api.interceptors.response.use(
  (response) => response, 
  (error) => {
    console.error("API Error:", error.response?.data || error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    const result = await api.get("/items");    
    return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name);
    const result = await api.post("/items", { name, isComplete: false });
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete });
    const result = await api.put(`/items/${id}`, { isComplete });
    return result.data;
  },

  deleteTask: async (id) => {
    console.log('deleteTask', id);
    await api.delete(`/items/${id}`);
  }
};