﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lam.zGame.Core_game.Core.Utilities.AntiCheat.ObscuredTypes
{
    /// <summary>
    /// Use it instead of regular <c>Vector2</c> for any cheating-sensitive variables.
    /// </summary>
    /// <strong>\htmlonly<font color="FF4040">WARNING:</font>\endhtmlonly Doesn't mimic regular type API, thus should be used with extra caution.</strong> Cast it to regular, not obscured type to work with regular APIs.<br/>
    /// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong>
    [Serializable]
    public struct ObscuredVector2
    {
        private static int cryptoKey = 120206;
        private static readonly Vector2 zero = Vector2.zero;

#if UNITY_EDITOR
        // For internal Editor usage only (may be useful for drawers).
        public static int cryptoKeyEditor = cryptoKey;
#endif

        [SerializeField]
        private int currentCryptoKey;

        [SerializeField]
        private RawEncryptedVector2 hiddenValue;

        [SerializeField]
        private bool inited;

        private ObscuredVector2(Vector2 value)
        {
            currentCryptoKey = cryptoKey;
            hiddenValue = Encrypt(value);

            inited = true;
        }

        /// <summary>
        /// Mimics constructor of regular Vector2.
        /// </summary>
        /// <param name="x">X component of the vector</param>
        /// <param name="y">Y component of the vector</param>
        public ObscuredVector2(float x, float y)
        {
            currentCryptoKey = cryptoKey;
            hiddenValue = Encrypt(x, y, currentCryptoKey);

            inited = true;
        }

        public float x
        {
            get
            {
                var decrypted = InternalDecryptField(hiddenValue.x);
                return decrypted;
            }

            set
            {
                hiddenValue.x = InternalEncryptField(value);
            }
        }

        public float y
        {
            get
            {
                var decrypted = InternalDecryptField(hiddenValue.y);
                return decrypted;
            }

            set
            {
                hiddenValue.y = InternalEncryptField(value);
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid ObscuredVector2 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid ObscuredVector2 index!");
                }
            }
        }

        /// <summary>
        /// Allows to change default crypto key of this type instances. All new instances will use specified key.<br/>
        /// All current instances will use previous key unless you call ApplyNewCryptoKey() on them explicitly.
        /// </summary>
        public static void SetNewCryptoKey(int newKey)
        {
            cryptoKey = newKey;
        }

        /// <summary>
        /// Use this simple encryption method to encrypt any Vector2 value, uses default crypto key.
        /// </summary>
        public static RawEncryptedVector2 Encrypt(Vector2 value)
        {
            return Encrypt(value, 0);
        }

        /// <summary>
        /// Use this simple encryption method to encrypt any Vector2 value, uses passed crypto key.
        /// </summary>
        public static RawEncryptedVector2 Encrypt(Vector2 value, int key)
        {
            return Encrypt(value.x, value.y, key);
        }

        /// <summary>
        /// Use this simple encryption method to encrypt Vector2 components, uses passed crypto key.
        /// </summary>
        public static RawEncryptedVector2 Encrypt(float x, float y, int key)
        {
            if (key == 0)
            {
                key = cryptoKey;
            }

            RawEncryptedVector2 result;
            result.x = ObscuredFloat.Encrypt(x, key);
            result.y = ObscuredFloat.Encrypt(y, key);

            return result;
        }

        /// <summary>
        /// Use it to decrypt RawEncryptedVector2 you got from Encrypt(), uses default crypto key.
        /// </summary>
        public static Vector2 Decrypt(RawEncryptedVector2 value)
        {
            return Decrypt(value, 0);
        }

        /// <summary>
        /// Use it to decrypt RawEncryptedVector2 you got from Encrypt(), uses passed crypto key.
        /// </summary>
        public static Vector2 Decrypt(RawEncryptedVector2 value, int key)
        {
            if (key == 0)
            {
                key = cryptoKey;
            }

            Vector2 result;
            result.x = ObscuredFloat.Decrypt(value.x, key);
            result.y = ObscuredFloat.Decrypt(value.y, key);

            return result;
        }

        /// <summary>
        /// Use it after SetNewCryptoKey() to re-encrypt current instance using new crypto key.
        /// </summary>
        public void ApplyNewCryptoKey()
        {
            if (currentCryptoKey != cryptoKey)
            {
                hiddenValue = Encrypt(InternalDecrypt(), cryptoKey);
                currentCryptoKey = cryptoKey;
            }
        }

        /// <summary>
        /// Allows to change current crypto key to the new random value and re-encrypt variable using it.
        /// Use it for extra protection against 'unknown value' search.
        /// Just call it sometimes when your variable doesn't change to fool the cheater.
        /// </summary>
        public void RandomizeCryptoKey()
        {
            var decrypted = InternalDecrypt();

            do
            {
                currentCryptoKey = Random.Range(int.MinValue, int.MaxValue);
            } while (currentCryptoKey == 0);
            hiddenValue = Encrypt(decrypted, currentCryptoKey);
        }

        /// <summary>
        /// Allows to pick current obscured value as is.
        /// </summary>
        /// Use it in conjunction with SetEncrypted().<br/>
        /// Useful for saving data in obscured state.
        public RawEncryptedVector2 GetEncrypted()
        {
            ApplyNewCryptoKey();
            return hiddenValue;
        }

        /// <summary>
        /// Allows to explicitly set current obscured value.
        /// </summary>
        /// Use it in conjunction with GetEncrypted().<br/>
        /// Useful for loading data stored in obscured state.
        public void SetEncrypted(RawEncryptedVector2 encrypted)
        {
            inited = true;
            hiddenValue = encrypted;

            if (currentCryptoKey == 0)
            {
                currentCryptoKey = cryptoKey;
            }
        }

        /// <summary>
        /// Alternative to the type cast, use if you wish to get decrypted value 
        /// but can't or don't want to use cast to the regular type.
        /// </summary>
        /// <returns>Decrypted value.</returns>
        public Vector2 GetDecrypted()
        {
            return InternalDecrypt();
        }

        private Vector2 InternalDecrypt()
        {
            if (!inited)
            {
                currentCryptoKey = cryptoKey;
                hiddenValue = Encrypt(zero);
                inited = true;

                return zero;
            }

            Vector2 value;

            value.x = ObscuredFloat.Decrypt(hiddenValue.x, currentCryptoKey);
            value.y = ObscuredFloat.Decrypt(hiddenValue.y, currentCryptoKey);

            return value;
        }

        private float InternalDecryptField(int encrypted)
        {
            var key = cryptoKey;

            if (currentCryptoKey != cryptoKey)
            {
                key = currentCryptoKey;
            }

            var result = ObscuredFloat.Decrypt(encrypted, key);
            return result;
        }

        private int InternalEncryptField(float encrypted)
        {
            var result = ObscuredFloat.Encrypt(encrypted, cryptoKey);
            return result;
        }

        #region operators, overrides, interface implementations
        //! @cond
        public static implicit operator ObscuredVector2(Vector2 value)
        {
            return new ObscuredVector2(value);
        }

        public static implicit operator Vector2(ObscuredVector2 value)
        {
            return value.InternalDecrypt();
        }

        public static implicit operator Vector3(ObscuredVector2 value)
        {
            var v = value.InternalDecrypt();
            return new Vector3(v.x, v.y, 0.0f);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// 
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return InternalDecrypt().GetHashCode();
        }

        /// <summary>
        /// Returns a nicely formatted string for this vector.
        /// </summary>
        public override string ToString()
        {
            return InternalDecrypt().ToString();
        }

        /// <summary>
        /// Returns a nicely formatted string for this vector.
        /// </summary>
        public string ToString(string format)
        {
            return InternalDecrypt().ToString(format);
        }

        //! @endcond
        #endregion

        /// <summary>
        /// Used to store encrypted Vector2.
        /// </summary>
        [Serializable]
        public struct RawEncryptedVector2
        {
            /// <summary>
            /// Encrypted value
            /// </summary>
            public int x;

            /// <summary>
            /// Encrypted value
            /// </summary>
            public int y;
        }

    }
}