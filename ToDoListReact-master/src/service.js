import axios from 'axios';

axios.defaults.baseURL = process.env.REACT_APP_API_URL

//טיפול בשגיאות:
axios.interceptors.response.use(
  //אם אין שגיאה
  (response)=>{
    return response
  },
  //אם יש שגיאה
  (error)=>{
    console.error("error in Api: ",error.response?.status,error.message);
    return Promise.reject(error);
  }  
);

export default {
  getTasks: async () => {
    const result = await axios.get(`/tasks`)    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
    const result=await axios.post(`/tasks`,{name,isComplete:false});
    return result.data;
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result=await axios.put(`/tasks/${id}`,{isComplete});
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask');
    const result=await axios.delete(`/tasks/${id}`);
    return result.data;
  }
};
