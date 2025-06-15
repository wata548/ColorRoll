namespace Extensions {
    public static class ExBool {
        public static int ToInt(this bool target) => target ? 1 : 0;
        public static bool ToBool(this int target) => target != 0;
    }
}