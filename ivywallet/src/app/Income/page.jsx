"use client";
import React, { useState, useEffect, useMemo } from "react";
import styles from "./income.module.css";
import {
            PieChart, Pie, Cell, Tooltip, Legend,
            ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid
            } from "recharts";
import Cookies from "js-cookie";
import { Calendar, ChevronDown, Wallet } from "lucide-react";
import ExpPopup from "../components/Popup/Popup";

const Income = () => {
  const [open, setOpen] = useState(false);
  const [userIncome, setUserIncome] = useState([]);
  const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
  const [isOpen_c, setIsOpen_c] = useState(false);
  const [total, setTotal] = useState(0);

  const months = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];

  const getUserIncome = async (month) => {
    try {
      const user_id = Cookies.get("user");
      const res = await fetch(
        `http://localhost:8000/api/Income/GetIncomeByMonth/${user_id}/${month + 1}`
      );
      const data = await res.json();
      setUserIncome(data || []);
    } catch (err) {
      console.error("Income fetch error:", err);
      setUserIncome([]);
    }
  };

  const getTotalIncome = async (month) => {
    try {
      const user_id = Cookies.get("user");
      const res = await fetch(
        `http://localhost:8000/api/Income/FullIncomeOfAUser/${user_id}/${month + 1}`
      );
      const data = await res.json();
      setTotal(data || 0);
    } catch (err) {
      console.error("Total income fetch error:", err);
      setTotal(0);
    }
  };

  // Pie data
  const pieData = useMemo(() => {
    return userIncome.map((item) => ({
      name: item.c_Name,
      value: Number(item.i_Amount) || 0,
    }));
  }, [userIncome]);

  // Bar chart data
  const barChartData = useMemo(() => {
    return userIncome.map((item) => ({
      name: item.c_Name,
      amount: Number(item.i_Amount) || 0,
      color: item.c_Color,
    }));
  }, [userIncome]);

  const COLORS = userIncome.map((item) => item.c_Color);

  const CustomTooltip = ({ active, payload }) => {
    if (active && payload && payload.length) {
      return (
        <div className={styles.customTooltip}>
          <p className={styles.tooltipLabel}>{payload[0].payload.name}</p>
          <p
            className={styles.tooltipValue}
            style={{ color: payload[0].fill }}
          >
            {Number(payload[0].value).toLocaleString()} BDT
          </p>
        </div>
      );
    }
    return null;
  };

  useEffect(() => {
    getUserIncome(selectedMonth);
    getTotalIncome(selectedMonth);
  }, [selectedMonth]);

  return (
    <div className={styles.container}>
      <div className={styles.nav}>
        <div className={styles.upper}>
          <div className={styles.titleWrapper}>
            <Wallet size={32} className={styles.iconBlue} />
            <div>
              <h1>Income</h1>
              <p className={styles.subtitle}>Track your income</p>
            </div>
          </div>

          <div className={styles.totalAmount}>
            <span className={styles.totalLabel}>Total</span>
            <div className={styles.totalValue}>{total} BDT</div>
          </div>
        </div>

        <div className={styles.navActions}>
        
          <div className={styles.monthSelectorWrapper}>
            <div
              className={styles.monthButton}
              onClick={() => setIsOpen_c(!isOpen_c)}
            >
              <Calendar size={20} />
              <span>{months[selectedMonth]}</span>
              <ChevronDown size={18} />
            </div>

            {isOpen_c && (
              <div className={styles.dropdown}>
                {months.map((month, index) => (
                  <div
                    key={month}
                    className={`${styles.dropdownItem} ${
                      selectedMonth === index ? styles.active : ""
                    }`}
                    onClick={() => {
                      setSelectedMonth(index);
                      setIsOpen_c(false);
                    }}
                  >
                    {month}
                  </div>
                ))}
              </div>
            )}
          </div>

    
          <div className={styles.img} onClick={() => setOpen(true)}>
            <img src="/add.png" height={23} width={23} />
          </div>
        </div>
      </div>

      <div className={styles.content}>
      
        <div className={styles.chartsGrid}>
        
          <div className={styles.chartCard}>
            <div className={styles.chartHeader}>
              <div className={`${styles.chartIndicator} ${styles.indicatorPie}`}></div>
              <h3 className={styles.chartTitle}>Income Distribution</h3>
            </div>

            <div className={styles.pie_chart}>
              <ResponsiveContainer width="100%" height={400}>
                <PieChart>
                  <Pie
                    data={pieData}
                    cx="50%"
                    cy="50%"
                    outerRadius={140}
                    dataKey="value"
                    label={(entry) =>
                      total > 0
                        ? `${entry.name}: ${(
                            (entry.value / total) *
                            100
                          ).toFixed(1)}%`
                        : `${entry.name}: 0%`
                    }
                  >
                    {pieData.map((entry, index) => (
                      <Cell
                        key={index}
                        fill={COLORS[index % COLORS.length]}
                      />
                    ))}
                  </Pie>
                  <Tooltip content={<CustomTooltip />} />
                </PieChart>
              </ResponsiveContainer>
            </div>
          </div>

    
          <div className={styles.chartCard}>
            <div className={styles.chartHeader}>
              <div className={`${styles.chartIndicator} ${styles.indicatorBar}`}></div>
              <h3 className={styles.chartTitle}>Category Breakdown</h3>
            </div>

            <div className={styles.bar_chart}>
              <ResponsiveContainer width="100%" height={400}>
                <BarChart data={barChartData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis
                    dataKey="name"
                    angle={-45}
                    textAnchor="end"
                    height={80}
                  />
                  <YAxis
                    tickFormatter={(value) => `${(value / 1000).toFixed(0)}k`}
                  />
                  <Tooltip content={<CustomTooltip />} />
                  <Bar dataKey="amount" radius={[8, 8, 0, 0]}>
                    {barChartData.map((entry, index) => (
                      <Cell key={index} fill={entry.color} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
        </div>

    
        <div className={styles.categorySection}>
          <div className={styles.sectionHeader}>
            <div className={`${styles.chartIndicator} ${styles.indicatorCategory}`}></div>
            <h3 className={styles.chartTitle}>Income Categories</h3>
          </div>

          <div className={styles.card_wrapper}>
            {userIncome.map((v) => (
              <div key={v.c_Name} className={styles.cards}>
                <div
                  className={styles.cardGlow}
                  style={{ background: `${v.c_Color}20` }}
                ></div>

                <div className={styles.cardContent}>
                  <div
                    className={styles.cardIcon}
                    style={{ background: `${v.c_Color}30` }}
                  >
                    <img src={v.c_Image} height={33} width={33} />
                  </div>

                  <div className={styles.card_title}>
                    <span>{v.c_Name}</span>
                    <h1 style={{ color: v.c_Color }}>
                      {Number(v.i_Amount || 0).toLocaleString()}
                    </h1>
                  </div>
                </div>

                <div className={styles.progressWrapper}>
                  <div
                    className={styles.progressBar}
                    style={{
                      width:
                        total > 0
                          ? `${((v.i_Amount || 0) / total) * 100}%`
                          : "0%",
                      background: v.c_Color
                    }}
                  ></div>
                </div>

                <p className={styles.percentText}>
                  {total > 0
                    ? `${(((v.i_Amount || 0) / total) * 100).toFixed(1)}% of total`
                    : "0% of total"}
                </p>
              </div>
            ))}
          </div>
        </div>
      </div>

      {open && <ExpPopup onClose={() => setOpen(false)} />}
    </div>
  );
};

export default Income;
