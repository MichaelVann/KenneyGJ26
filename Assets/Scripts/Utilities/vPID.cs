using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class vPID
{
    float m_p, m_i, m_d;
    float m_integral, m_lastError;

    internal void SetVariables(float a_p, float a_i, float a_d) { m_p = a_p; m_i = a_i; m_d = a_d; }

    internal vPID(float a_p, float a_i, float a_d)
    {
        SetVariables(a_p, a_i, a_d);
    }


    internal float Update(float a_current, float a_target)
    {
        float result = 0f;
        float error = a_target - a_current;
        float deltaTime = Time.deltaTime;

        m_integral += error * deltaTime;
        float derivative = (error - m_lastError) / deltaTime;
        m_lastError = error;

        result = m_p * error + m_i * m_integral + m_d * derivative;

        return result;
    }

    internal Vector3 Update(Vector3 a_current,Vector3 a_target)
    {
        float result = 0f;
        Vector3 deltaVector = a_target - a_current;
        float error = (deltaVector).magnitude;
        float deltaTime = Time.deltaTime;

        m_integral += error * deltaTime;
        float derivative = (error - m_lastError) / deltaTime;
        m_lastError = error;

        result = m_p * error + m_i * m_integral + m_d * derivative;

        return deltaVector.normalized * result;
    }

}
