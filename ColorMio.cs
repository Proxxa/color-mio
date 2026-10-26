using MioGame;
using MioGame.Shader;
using MioModdingApi;
using MioModLoader;

namespace ColorMio
{
    public class ColorMio : Mod
    {
        public Vec_float_3 color = default;


        public override void Initialize()
        {
            Hooks();

            ConnectMenus();
        }
        
        private static ModMenuPage? modPage;
        private static NumberOption? rTint;
        private static NumberOption? gTint;
        private static NumberOption? bTint;

        public void ConnectMenus()
        {
            modPage = ModMenu.RegisterMod(this, "Color Mio", "Color Mio");

            rTint = modPage.RegisterNumber(
                "r_tint", "Tint (Red)",
                0, 255, 1, 1,
                "The red value of the tentacle color."
                );
            rTint.Changed += (int value) => color.x = value;

            gTint = modPage.RegisterNumber(
                "g_tint", "Tint (Green)",
                0, 255, 1, 1,
                "The green value of the tentacle color."
                );
            gTint.Changed += (int value) => color.y = value;

            bTint = modPage.RegisterNumber(
                "b_tint", "Tint (Blue)",
                0, 255, 1, 1,
                "The blue value of the tentacle color."
                );
            bTint.Changed += (int value) => color.z = value;
        }

        private unsafe void Hooks()
        {
            On.MioGame.GlobalFunctions.game.On_renderer.render_tentacle_2.Prefix += Render_tentacle_prefix;
            On.MioGame.On_Game.fixed_update.Prefix += Fixed_update_prefix;
        }

        private unsafe void Fixed_update_prefix(Game* __this)
        {
            ref var mio = ref __this->mio;
            fixed (Mio* ptr = &mio) 
            {
                MioGame.GlobalFunctions.game.mio.mio_head_color(ptr);
            }
            
        }

        private unsafe void Render_tentacle_prefix(uint idx, Mio.Tentacle* t, Vec_float_3 chain_u, Vec_float_3 chain_v, 
            Array_Vec_float_3* chain, Array_Shader__Tentacle_vertex* vertices, Array_unsigned_int* indices, 
            Tentacle_consts* consts)
        {
            ref var tint = ref consts->tint;
            tint.x = color.x;
            tint.y = color.y;
            tint.z = color.z;
            
            //PrintVector(ref tint);
        }

        private void PrintVector(ref Vec_float_3 vec)
        {
            LogMessage("x: " + vec.x.ToString());
            LogMessage("y: " + vec.y.ToString());
            LogMessage("z: " + vec.z.ToString());
        }
    }
}
