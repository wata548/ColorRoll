namespace Extensions {
    public static class ExSingle {

        public static float Clamp(float min, float target, float max) {
            if (target < min)
                target = min;
            if (target > max)
                target = max;
            return target;
        }
    }
}