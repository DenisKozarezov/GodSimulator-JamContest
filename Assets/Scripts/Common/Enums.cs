namespace Core
{
    public enum FadeMode : byte { On, Off }

    namespace UI
    {
        public enum CursorType : byte
        {
            Default = 0x00,
            Target = 0x01,
            Hover = 0x02,
            Disabled = 0x04,
            Ability = 0x08,
        }
        enum CursorSize : byte
        {
            _16x16 = 0x00,
            _32x32 = 0x01,
            _64x64 = 0x02,
            _128x128 = 0x04,
        }
    }
}
