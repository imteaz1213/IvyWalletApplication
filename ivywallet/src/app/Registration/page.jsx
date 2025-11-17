"use client"
import React from "react";
import styles from "./registration.module.css";
import { useState,useEffect } from "react";


const Registration = () => { 
    const [userName,setUserName] = useState("");
    const [password,setPassword] = useState("");
    const [email,setEmail] = useState("");
    const [address,setAddress] = useState("");
    const [currency,setCurrency] = useState([]);
    const [phone,setPhone] = useState("");
    const [curr_id,setCurr_id] = useState(0);

    const Getcurrency = async () => {
       try {
          const res = await fetch(`http://localhost:8000/api/Home/GetCurrency`, 
          {
            method: "GET"
          }
        );
          const data = await res.json();
          setCurrency(data);
        } 
        catch (error) {
          console.error(error);
        }
    }
    
    const Register = async (data) => {
        try
        {
            const response = await fetch(`http://localhost:8000/api/Auth/Register/register`,
                {
                    method:"POST",
                    headers: { "Content-Type": "application/json"},
                    body: JSON.stringify(data)
                }
            );
            console.log(response);
            if (!response.ok) 
                throw new Error("Registration failed");
            alert("Registered");
        }
        catch(e)
        {
            console.error("Failed",e);
        }
    };
    
    const handle = () => {
        const user = { 
            name:userName, 
            email:email, 
            password:password, 
            address:address, 
            phone:phone,
            currencyId:Number(curr_id)
        };
        Register(user);
    };


    useEffect(() => {
        Getcurrency();
    }, []);
    
    
    return (
        <div className={styles.main}>
            <div className={styles.container}>
            <div className={styles.title}>
                <h1>Registration</h1>
            </div> 
            <div className={styles.form}>
                <div className={styles.row}>
                        <label> Name </label> 
                        <div>
                            <input type="text" onChange={(e)=> setUserName(e.target.value)} value={userName}  className={styles.input}/>
                        </div>
                </div>
                <div className={styles.row}>
                        <label> Password </label> 
                        <div>
                            <input type="password" onChange={(e)=> setPassword(e.target.value)} value={password} className={styles.input}/>
                        </div>
                </div> 
                <div className={styles.row}>
                        <label > Email </label> 
                        <div>
                            <input type="email" onChange={(e)=> setEmail(e.target.value)} value={email} className={styles.input}/>
                        </div>
                </div>
                <div className={styles.row}>
                        <label> Address </label> 
                        <div>
                            <input type="text" onChange={(e)=>setAddress(e.target.value)} value={address} className={styles.input}/>
                        </div>
                </div>
                <div className={styles.row}>
                        <label> Currency </label> 
                        <select value={curr_id} onChange={(e) => setCurr_id(Number(e.target.value))} className={styles.input}>
                            <option value="">Choose Currency...</option>
                            {currency.map((c) => (
                                <option key={c.id} value={c.id}>{c.title}</option>
                            ))}
                        </select>
                </div>
                <div className={styles.row}>
                        <label> Phone </label> 
                        <div>
                            <input type="text" onChange={(e)=>setPhone(e.target.value)} value={phone} className={styles.input}/>
                        </div>
                </div>
                <div className={styles.row}>
                    <button type="button" onClick={handle} className={styles.btn}>Submit</button>
                </div>        
            </div>
        </div>
        <div style={{height:50}}></div>
        </div>
      );
};
export default Registration;