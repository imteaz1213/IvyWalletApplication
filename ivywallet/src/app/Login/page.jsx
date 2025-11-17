"use client"
import React  from "react";
import styles from "./login.module.css"
import { useState,useEffect } from "react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";

const Login = () => { 
    const router = useRouter();
    const [email,setEmail] = useState("");
    const [password,setPassword] = useState("");

    const login = async (data) => {
        try 
        {
            const response = await fetch(`http://localhost:8000/api/Auth/Login/login`,
                {
                    method:"POST",
                    headers: { "Content-Type": "application/json"},
                    body: JSON.stringify(data),
                }
            );
            if (!response.ok) 
                alert("Invalid credentials!");
            
            const result = await response.json();
            Cookies.set("jwtToken", result.token, { expires: 1 });
            Cookies.set("user",result.user.id,{ expires: 1 });
            router.push("/Main");
        } 
        catch (error) 
        {
            console.error(error);
        }
      };


      const handle = () => {
            const cred = {
                email:email,
                password:password
            };
            login(cred);
      };

      const Go = () => {
            router.push("/Registration");
      };

      return (
      <div className={styles.main}>
          <div className={styles.container}>
            <div className={styles.row}>
                        <label> Email </label> 
                        <div>
                            <input type="email" onChange={(e)=>setEmail(e.target.value)} value={email} className={styles.input}/>
                        </div>
                </div>
                <div className={styles.row}>
                        <label> Password </label> 
                        <div>
                            <input type="password" onChange={(e)=>setPassword(e.target.value)} value={password} className={styles.input}/>
                        </div>
                </div>
                <div className={styles.row}>
                    <button type="button" onClick={handle} className={styles.btn}>LogIn</button>
                </div>    
                <div onClick={Go} className={styles.footer}>
                    <div className={styles.footer_t}>Don't have any account ?</div>
                    <div className={styles.footer_btn}>
                        <button>Register</button>
                    </div>
                </div>
        </div>
        </div>
      );
};
export default Login;