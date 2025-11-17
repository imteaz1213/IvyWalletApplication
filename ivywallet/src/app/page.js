"use client"
import Cookies from "js-cookie";
import Main from "./Main/page";
import Login from "./Login/page";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "next/navigation";
import { useEffect } from "react";


export default function Home() { 
   const router = useRouter();
   
  useEffect(() => {
        const token = Cookies.get("jwtToken");

        if (!token) {
        router.push("/Login");
        return;
        }

    try {
      const decoded = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      
      if (decoded.exp < currentTime) {
        Cookies.remove("jwtToken");
        router.push("/Login");
      } 
      else {
        router.push("/Main");
      }
      } 
    catch {
      router.push("/Login");
    } 
  }, []);

  return null;
};

