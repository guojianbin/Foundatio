﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundatio.Utility;

namespace Foundatio.Caching {
    public static class CacheClientExtensions {
        public static async Task<T> GetAsync<T>(this ICacheClient client, string key, T defaultValue) {
            var cacheValue = await client.GetAsync<T>(key).AnyContext();
            return cacheValue.HasValue ? cacheValue.Value : defaultValue;
        }

        public static Task<IDictionary<string, CacheValue<T>>> GetAllAsync<T>(this ICacheClient client, params string[] keys) {
            return client.GetAllAsync<T>(keys.ToArray());
        }

        public static async Task<bool> RemoveAsync(this ICacheClient client, string key) {
            return await client.RemoveAllAsync(new[] { key }).AnyContext() == 1;
        }

        public static Task<long> IncrementAsync(this ICacheClient client, string key, long amount, DateTime expiresAtUtc) {
            return client.IncrementAsync(key, amount, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }

        public static Task<double> IncrementAsync(this ICacheClient client, string key, double amount, DateTime expiresAtUtc) {
            return client.IncrementAsync(key, amount, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }

        public static Task<long> IncrementAsync(this ICacheClient client, string key, TimeSpan? expiresIn = null) {
            return client.IncrementAsync(key, 1, expiresIn);
        }

        public static Task<long> DecrementAsync(this ICacheClient client, string key, TimeSpan? expiresIn = null) {
            return client.IncrementAsync(key, -1, expiresIn);
        }

        public static Task<long> DecrementAsync(this ICacheClient client, string key, long amount, TimeSpan? expiresIn = null) {
            return client.IncrementAsync(key, -amount, expiresIn);
        }
        
        public static Task<bool> AddAsync<T>(this ICacheClient client, string key, T value, DateTime expiresAtUtc) {
            return client.AddAsync(key, value, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }

        public static Task<bool> SetAsync<T>(this ICacheClient client, string key, T value, DateTime expiresAtUtc) {
            return client.SetAsync(key, value, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }

        public static Task<bool> ReplaceAsync<T>(this ICacheClient client, string key, T value, DateTime expiresAtUtc) {
            return client.ReplaceAsync(key, value, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }
        
        public static Task<int> SetAllAsync(this ICacheClient client, IDictionary<string, object> values, DateTime expiresAtUtc) {
            return client.SetAllAsync(values, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }
        
        public static Task SetExpirationAsync(this ICacheClient client, string key, DateTime expiresAtUtc) {
            return client.SetExpirationAsync(key, expiresAtUtc.Subtract(SystemClock.UtcNow));
        }

        public static async Task<bool> SetAddAsync<T>(this ICacheClient client, string key, T value, TimeSpan? expiresIn = null) {
            return await client.SetAddAsync(key, new [] { value }, expiresIn).AnyContext() > 0;
        }

        public static async Task<bool> SetRemoveAsync<T>(this ICacheClient client, string key, T value, TimeSpan? expiresIn = null) {
            return await client.SetRemoveAsync(key, new[] { value }, expiresIn).AnyContext() > 0;
        }

        public static Task<long> SetIfHigherAsync(this ICacheClient client, string key, DateTime value, TimeSpan? expiresIn = null) {
            long unixTime = value.ToUnixTimeSeconds();
            return client.SetIfHigherAsync(key, unixTime, expiresIn);
        }

        public static Task<long> SetIfLowerAsync(this ICacheClient client, string key, DateTime value, TimeSpan? expiresIn = null) {
            long unixTime = value.ToUnixTimeSeconds();
            return client.SetIfLowerAsync(key, unixTime, expiresIn);
        }
        
        public static async Task<DateTime> GetUnixTimeSecondsAsync(this ICacheClient client, string key, DateTime? defaultValue = null) {
            var unixTime = await client.GetAsync<long>(key).AnyContext();
            if (!unixTime.HasValue)
                return defaultValue ?? DateTime.MinValue;

            return unixTime.Value.FromUnixTimeSeconds();
        }

        public static Task<bool> SetUnixTimeSecondsAsync(this ICacheClient client, string key, DateTime value, TimeSpan? expiresIn = null) {
            return client.SetAsync(key, value.ToUnixTimeSeconds(), expiresIn);
        }

        public static Task<bool> SetUnixTimeSecondsAsync(this ICacheClient client, string key, DateTime value, DateTime expiresAtUtc) {
            return client.SetAsync(key, value.ToUnixTimeSeconds(), expiresAtUtc.Subtract(SystemClock.UtcNow));
        }
    }
}
