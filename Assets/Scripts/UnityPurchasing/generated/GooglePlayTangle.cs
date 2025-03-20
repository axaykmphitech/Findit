// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("AX5J+VBMNhzSWJS43eLGcz3TujN5ZLEv+taS9un5ZMBPFx/ZTcHSoacvqQs9GYF+baEJNK5NPtbB9KeLlpWs02xMcmQbY5FeoSGWsXDnh+D/fHJ9Tf98d3//fHx97wMbtpkDpikstauojvWG4uQFSuIHVlU0yd5z5zMNgrmsZ1HfltEQZP2W9FSybP1N/3xfTXB7dFf7NfuKcHx8fHh9fqFMEqcUtKTBnhUcPfrBy9aoYdA/pRZOENXztnkSHna4YjqG8U5mqHKxHUgTK1vRfz0nm9CHkVbGXKfzrgHlR7IFzPk+tmy+ao+nXvtTgXOfZuzka5a6kgCMo4JMGe9FV+XiNKbES6+R8lorQV+gK7ChFKGfbfVMOXgufbpOaOTXnn9+fH18");
        private static int[] order = new int[] { 13,4,10,3,4,6,11,13,10,11,11,13,13,13,14 };
        private static int key = 125;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
