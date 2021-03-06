﻿using NeoFPS.AI.Condition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.AI.Behaviour
{
    /// <summary>
    /// An abstract Scriptable Object describing an AI Behaviour that an NPC might exhibit.
    /// </summary>
    public abstract class AIBehaviour : AIScriptableObject
    {
        [SerializeField, Tooltip("List of conditions that should be met if this behaviour is to fire.")]
        internal AICondition[] m_Conditions;

        bool m_IsActive = true;
        /// <summary>
        /// Is this behaviour active?
        /// </summary>
        internal bool IsActive
        {
            get {
                if (!m_IsActive) return false;

                for (int i = 0; i < m_Conditions.Length; i++)
                {
                    if (!m_Conditions[i].GetResult())
                    {
                        return false;
                    }
                }
                return true;
            }
            set { m_IsActive = value; }
        }

        /// <summary>
        /// The owning NPC for this AI Behaviour instance.
        /// </summary>
        internal GameObject m_Owner = null;
        /// <summary>
        /// The controller that manages this behaviour.
        /// </summary>
        internal BasicAIController m_Controller;

        /// <summary>
        /// Called during the AIController Start method to initialize any components needed.
        /// <param name="owner">The parent GameObject for this behaviour.</param>
        /// <return>True if the behaviour has been correctly initialized.</return>
        /// </summary>
        internal virtual bool Init(GameObject owner, BasicAIController controller)
        {
            m_Owner = owner;
            m_Controller = controller;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                m_Conditions[i] = Instantiate(m_Conditions[i]); // instantiate so that a single SO is not shared across GameObjects
                m_Conditions[i].Init(this);
            }
            return m_IsActive;
        }

        /// <summary>
        /// Called on each update tick by the controller. This may be less frequently than
        /// the Update cycle depending on the controller configuration. Use this method to 
        /// take any action required.
        /// </summary>
        /// <returns>An empty string if the behaviour fired or a reason that the behaviour did not fire.</returns>
        internal abstract string Tick();
    }
}
