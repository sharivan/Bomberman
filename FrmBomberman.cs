using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Geometry2D;
using System.Reflection;
using System.IO;
using System.Media;
using System.Diagnostics;
using System.Net.Sockets;

namespace Bomberman
{
    public partial class FrmBomberman : Form
    {
        public interface INetworkObject
        {
            void Read(BinaryReader reader);
            void Write(BinaryWriter writer);
        }

        /// <summary>
        /// Permite a renderização de uma animação na tela do jogo.
        /// Usada pelas entidades do engine.
        /// </summary>
        public class Animation : IDisposable
        {
            private Entity entity; // Entidade que possui esta animação
            private int index; // Índice desta animação
            private ImageList imageList; // ImageList usado para gerar a animação (cada elemento do ImageList é um frame desta animação)
            private float fps; // Quantidade de quadros por segundo utilizados para exibir a animação
            private int initialFrame; // Quadro inicial da animação
            private bool visible; // Indica se a animação será visivel (se ela será renderizada ou não)
            private bool animating; // Indica se a animação será dinâmica ou estática
            private bool loop; // Indica se a animação ocorrerá em looping

            private int currentFrame; // Quadro atual
            private int tick; // Quantidade de ticks desde a criação da animação
            private int nextTick; // Próximo tick no qual deverá ocorrer a renderização do próximo quadro
            private bool flashing; // Indica se a animação será exibia com o efeito piscante (usado pelos sprites no modo de invencibilidade)
            private bool bright; // Indica se o quadro atual da animação estará com o brilho alto ou normal
            private float nextBrightTick; // Proximo tick no qual deverá ocorrer a alternância do estado bright
            private bool animationEndFired; // Indica se o evento OnAnimationEnd da entidade associada a esta animação foi chamado desde que a animação foi completada

            private Bitmap[] bitmaps; // Cache dos frames
            private Bitmap[] brightBitmaps; // Cache dos frames com brilho
            private ColorMatrix cmxPic; // Matriz de cores usada para a geração do efeito de transparência, usado pelo efeito fading da entidade
            private ImageAttributes iaPic; // Atributos de imagem usada para a geração do efeito de transparência

            /// <summary>
            /// Cria uma nova animação.
            /// A quantidade de quadros por segundo será definida por padrão pela constante DEFAULT_FPS.
            /// </summary>
            /// <param name="entity">Entidade possuidora da animação</param>
            /// <param name="index">Índice da animação</param>
            /// <param name="imageList">ImageList usado para gerar a animação (cada elemento do ImageList é um frame desta animação)</param>
            /// <param name="initialFrame">Quadro inicial da animação</param>
            /// <param name="startVisible">Especifica se a animação iniciará visível ou não</param>
            /// <param name="startOn">Especifica se a animação começará ativa ou não</param>
            /// <param name="loop">Especifica se a animação estará em looping ou não</param>
            public Animation(Entity entity, int index, ImageList imageList, int initialFrame = 0, bool startVisible = true, bool startOn = true, bool loop = true) :
            this(entity, index, imageList, DEFAULT_FPS, initialFrame, startVisible, startOn, loop)
            {
            }

            /// <summary>
            /// Cria uma nova animação
            /// </summary>
            /// <param name="entity">Entidade possuidora da animação</param>
            /// <param name="index">Índice da animação</param>
            /// <param name="imageList">ImageList usado para gerar a animação (cada elemento do ImageList é um frame desta animação)</param>
            /// <param name="fps">Quandidade de quadros por segundo da animação</param>
            /// <param name="initialFrame">Quadro inicial da animação</param>
            /// <param name="startVisible">Especifica se a animação iniciará visível ou não</param>
            /// <param name="startOn">Especifica se a animação começará ativa ou não</param>
            /// <param name="loop">Especifica se a animação estará em looping ou não</param>
            public Animation(Entity entity, int index, ImageList imageList, float fps, int initialFrame = 0, bool startVisible = true, bool startOn = true, bool loop = true)
            {
                this.entity = entity;
                this.index = index;
                this.imageList = imageList;
                this.fps = fps;
                this.initialFrame = initialFrame;
                visible = startVisible;
                animating = startOn;
                this.loop = loop;

                currentFrame = initialFrame; // Define o frame atual para o frame inicial
                tick = 0; // Reseta o número de ticks
                nextTick = (int) (TICKRATE / fps); // Calcula quando deverá ser o próximo tick para que ocorra a troca de quadros
                bright = false;
                nextBrightTick = 0;
                animationEndFired = false;

                cmxPic = new ColorMatrix();
                iaPic = new ImageAttributes();

                Precache(); // Inicializa o cache dos frames
            }

            /// <summary>
            /// Inicializa o cache dos frames da animação.
            /// Com o uso desta técnica a renderização da animação é acelerada durante cada repintura.
            /// </summary>
            private void Precache()
            {
                int count = imageList.Images.Count;
                bitmaps = new Bitmap[count];
                brightBitmaps = new Bitmap[count];

                for (int i = 0; i < count; i++)
                {
                    bitmaps[i] = PrecacheBitmap(imageList.Images[i], false);
                    brightBitmaps[i] = PrecacheBitmap(imageList.Images[i], true);
                }
            }

            /// <summary>
            /// Realiza o precache de uma imagem específica
            /// </summary>
            /// <param name="image">Imagem</param>
            /// <param name="bright">true se ela estará brilhando, falso se estará normal</param>
            /// <returns>Imagem no formato bitmap</returns>
            private Bitmap PrecacheBitmap(Image image, bool bright)
            {
                Box2D drawBox = entity.DrawBox;
                Rectangle rect = new Rectangle(0, 0, (int) drawBox.Width, (int) drawBox.Height);
                Bitmap bitmap = new Bitmap(rect.Width, rect.Height);
                Graphics g = Graphics.FromImage(bitmap);

                if (entity.Tiled) // Se a propriedade Tiles da entidade for verdadeira, desenha a animação lado a lado de forma a preencher toda a imagem
                    using (TextureBrush brush = new TextureBrush(image, WrapMode.Tile))
                    {
                        g.FillRectangle(brush, rect);
                    }
                else
                {
                    if (bright) // Se bright for definito como true, aplica o efeito de brilho intenso na imagem
                    {
                        float brightness = 10; // raise the brightness in 10x
                        float contrast = 1; // no change the contrast
                        float gamma = 1; // no change in gamma

                        float[][] ptsArray =
                        {
                        new float[] {contrast, 0, 0, 0, 0}, // scale red
                        new float[] {0, contrast, 0, 0, 0}, // scale green
                        new float[] {0, 0, contrast, 0, 0}, // scale blue
                        new float[] {0, 0, 0, 1, 0}, // scale alpha
                        new float[] {brightness, brightness, brightness, 0, 1}
                    };

                        using (ImageAttributes imageAttributes = new ImageAttributes())
                        {
                            imageAttributes.ClearColorMatrix();
                            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

                            g.DrawImage(image, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, imageAttributes);
                        }
                    }
                    else
                        g.DrawImage(image, rect); // Senão simplesmente desenha a imagem sem modificações
                }

                return bitmap;
            }

            /// <summary>
            /// Inicia a animação a partir do quadro atual
            /// </summary>
            public void Start()
            {
                animationEndFired = false;
                animating = true;
                nextTick = tick + (int) (TICKRATE / fps); // calcula o próximo tick no qual deverá ocorrer a troca de quadros
                entity.Engine.Repaint(entity); // Notifica o engine que a entidade deverá ser redesenhada na tela
            }

            /// <summary>
            /// Inicia a animação a partir do quadro inicial
            /// </summary>
            public void StartFromBegin()
            {
                Reset();
                Start();
            }

            /// <summary>
            /// Para a animação
            /// </summary>
            public void Stop()
            {
                animating = false;
                entity.Engine.Repaint(entity);
            }

            /// <summary>
            /// Reseta a animação, definindo o quadro atual como o quadro inicial
            /// </summary>
            public void Reset()
            {
                currentFrame = initialFrame;
                entity.Engine.Repaint(entity);
            }

            /// <summary>
            /// Libera todos os recursos associados a esta animação.
            /// Execute este método somente quando não for usar mais este objeto.
            /// </summary>
            public void Dispose()
            {
                foreach (Bitmap bitmap in bitmaps)
                    bitmap.Dispose();

                foreach (Bitmap bitmap in brightBitmaps)
                    bitmap.Dispose();

                iaPic.Dispose();
            }

            /// <summary>
            /// Entidade possuidora da animação
            /// </summary>
            public Entity Entity
            {
                get
                {
                    return entity;
                }
            }

            /// <summary>
            /// Índice da animação
            /// </summary>
            public int Index
            {
                get
                {
                    return index;
                }
            }

            /// <summary>
            /// ImageList usado para gerar a animação (cada elemento do ImageList é um frame desta animação)
            /// </summary>
            public ImageList ImageList
            {
                get
                {
                    return imageList;
                }
            }

            /// <summary>
            /// Quantidde de quadros por segundo da animação
            /// </summary>
            public float FPS
            {
                get
                {
                    return fps;
                }
                set
                {
                    fps = value;
                }
            }

            /// <summary>
            /// Frame inicial da animação
            /// </summary>
            public int InitialFrame
            {
                get
                {
                    return initialFrame;
                }
                set
                {
                    initialFrame = value;
                }
            }

            /// <summary>
            /// Frame atual da animação
            /// </summary>
            public int CurrentFrame
            {
                get
                {
                    return currentFrame;
                }
                set
                {
                    if (value >= imageList.Images.Count)
                        currentFrame = imageList.Images.Count - 1;
                    else
                        currentFrame = value;

                    animationEndFired = false;
                    entity.Engine.Repaint(entity);
                }
            }

            /// <summary>
            /// Visibilidade da animação (true se está visível, false caso contrário)
            /// </summary>
            public bool Visible
            {
                get
                {
                    return visible;
                }
                set
                {
                    visible = value;
                    entity.Engine.Repaint(entity);
                }
            }

            /// <summary>
            /// true se a animação está sendo executada, false caso contrário
            /// </summary>
            public bool Animating
            {
                get
                {
                    return animating;
                }
                set
                {
                    if (value && !animating)
                        Start();
                    else if (!value && animating)
                        Stop();
                }
            }

            /// <summary>
            /// true se a animação está ou será executada em looping, false caso contrário
            /// </summary>
            public bool Loop
            {
                get
                {
                    return loop;
                }
                set
                {
                    loop = value;
                }
            }

            /// <summary>
            /// Especifica se a animação será executada com o efeito pisca pisca com seu brilho sendo alternado.
            /// Usado pelos sprites para indicar que estão no modo de invencibilidade.
            /// </summary>
            public bool Flashing
            {
                get
                {
                    return flashing;
                }
                set
                {
                    flashing = value;

                    if (visible)
                        entity.Engine.Repaint(entity);
                }
            }

            /// <summary>
            /// Evento a ser executado a cada frame (tick) do engine
            /// </summary>
            public void OnFrame()
            {
                tick++; // Incrementa o número de ticks da animação

                // Verifica se o efeito pisca pisca está ativo e realiza as operações de alternância do brilho
                if (flashing)
                {
                    if (tick >= nextBrightTick)
                    {
                        bright = !bright;
                        nextBrightTick = tick + BRIGHT_TICK;

                        if (visible)
                            entity.Engine.Repaint(entity);
                    }
                }
                else if (bright)
                {
                    bright = false;

                    if (visible)
                        entity.Engine.Repaint(entity);
                }

                // Se a animação não está em execução ou não ouver pelo menos dois quadros na animação então não é necessário computar o próximo quadro da animação
                if (!animating || imageList.Images.Count <= 1)
                    return;

                // Verifica se está na hora de avançar o quadro da animação.
                if (tick >= nextTick)
                {
                    if (currentFrame >= imageList.Images.Count - 1) // Se chegamos no final da animação
                    {
                        if (!animationEndFired)
                        {
                            animationEndFired = true;
                            entity.OnAnimationEnd(this);
                        }

                        if (loop) // e se a animação está em looping, então o próximo frame deverá ser o primeiro frame da animação (não o frame inicial, definido por initialFrame)
                        {
                            currentFrame = 0;
                            animationEndFired = false;
                        }
                        else if (currentFrame > imageList.Images.Count - 1) // Por via das dúvidas, verifique se o frame atual não passou dos limites
                            currentFrame = imageList.Images.Count - 1;
                    }
                    else // Senão, avança para o próximo frame
                        currentFrame++;

                    nextTick = tick + (int) (TICKRATE / fps); // e computa quando deverá ocorrer o próximo avanço de frame

                    if (visible) // Se a animação estiver visível, notifica o engine que a entidade possuidora dela deverá ser redesenhado
                        entity.Engine.Repaint(entity);
                }
            }

            /// <summary>
            /// Realiza a pintura da animação
            /// </summary>
            /// <param name="g">Objeto do tipo Graphics usado nas operações de desenho e pintura pela animação</param>
            public void Paint(Graphics g)
            {
                // Se ñão estiver visível ou não ouver frames então não precisa desenhar nada
                if (!visible || imageList.Images.Count == 0)
                    return;

                Box2D drawBox = entity.DrawBox; // Obtém o retângulo de desenho da entidade
                Bitmap bitmap = bright ? brightBitmaps[currentFrame] : bitmaps[currentFrame]; // Obtém o frame a ser desenhado a partir do cache
                cmxPic.Matrix33 = entity.Opacity; // Atualiza a opacidade da imagem
                iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); // Define a matriz de cores da imagem
                g.DrawImage(bitmap, entity.Engine.TransformBox(drawBox), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, iaPic); // Desenha o frame
            }
        }

        /// <summary>
        /// Entidade do engine.
        /// É a base de todos os elementos do jogo, sejam blocos estáticos ou sprites (elementos móveis).
        /// </summary>
        public abstract class Entity : IDisposable, INetworkObject
        {
            protected FrmBomberman engine; // Engine
            protected string name; // Nome da entidade
            protected Box2D drawBox; // Retângulo de desenho
            protected Box2D collisionBox; // Retângulo de colisão
            protected List<Animation> animations; // Animações
            private bool tiled; // Especifica se a imagem (ou a animação) desta entidade será desenhanda com preenchimento lado a lado dentro da área de desenho
            private float opacity; // Opacidade da imagem (ou animação). Usada para efeito de fading.
            protected bool solid; // Especifica se a entidade será solida ou não a outros elementos do jogo.
            private bool fading; // Especifica se o efeito de fading está ativo
            private bool fadingIn; // Se o efeito de fading estiver ativo, especifica se o tipo de fading em andamento é um fading in
            private float fadingTime; // Se o efeito de fading estiver ativo, especifica o tempo do fading
            private float elapsed; // Se o efeito de fading estiver ativo, indica o tempo decorrido desde o início do fading
            protected Vector2D lastDelta; // Indica o deslocamento desta entidade desde o tick anterior
            protected bool disposed; // Indica se os recursos associados a esta entidade foram liberados
            private int drawCount; // Armazena a quantidade de pinturas feita pela entidade desde sua criação. Usado somente para depuração.

            private ImageList[] imageLists; // Array de lista de imagens a ser usada na animação desta entidade

            /// <summary>
            /// Cria uma nova entidade
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome da entidade</param>
            /// <param name="box">Retângulo de desenho e colisão</param>
            /// <param name="imageLists">Array de lista de imagens usada na animação</param>
            /// <param name="tiled">true se o desenho desta entidade será preenchido em sua área de pintura lado a lado</param>
            protected Entity(FrmBomberman engine, string name, Box2D box, ImageList[] imageLists, bool tiled = false)
            : this(engine, name, box, box, imageLists, tiled)
            {
            }

            /// <summary>
            /// Cria uma nova entidade
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome da entidade</param>
            /// <param name="drawBox">Retângulo de desenho</param>
            /// <param name="collisionBox">Retângulo de colisão</param>
            /// <param name="imageLists">Array de lista de imagens usada na animação</param>
            /// <param name="tiled">true se o desenho desta entidade será preenchido em sua área de pintura lado a lado</param>
            protected Entity(FrmBomberman engine, string name, Box2D drawBox, Box2D collisionBox, ImageList[] imageLists, bool tiled = false)
            {
                this.engine = engine;
                this.name = name;
                this.drawBox = drawBox;
                this.collisionBox = collisionBox;
                this.imageLists = imageLists;
                this.tiled = tiled;

                opacity = 1; // Opacidade 1 significa que não existe transparência (opacidade 1 = opacidade 100% = transparência 0%)
            }

            /// <summary>
            /// Libera qualquer recurso associado a esta entidade.
            /// Utilize este método somente quando este objeto não for mais utilizado.
            /// </summary>
            public virtual void Dispose()
            {
                if (disposed)
                    return;

                foreach (Animation animation in animations)
                    animation.Dispose();

                animations.Clear();
                disposed = true;
            }

            /// <summary>
            /// Evento interno que ocorrerá toda vez que uma animação estiver a ser criada.
            /// Seus parâmetros (exceto animationIndex) são passados por referencia de forma que eles podem ser alterados dentro do método e assim definir qual será o comportamento da animação antes que ela seja criada.
            /// </summary>
            /// <param name="animationIndex">Índice da animação</param>
            /// <param name="imageList">Lista de imagens contendo cada quadro usado pela animação</param>
            /// <param name="fps">Número de quadros por segundo</param>
            /// <param name="initialFrame">Quadro inicial</param>
            /// <param name="startVisible">true se a animação iniciará visível, false caso contrário</param>
            /// <param name="startOn">true se a animação iniciará em execução, false caso contrário</param>
            /// <param name="loop">true se a animação executará em looping, false caso contrário</param>
            protected virtual void OnCreateAnimation(int animationIndex, ref ImageList imageList, ref float fps, ref int initialFrame, ref bool startVisible, ref bool startOn, ref bool loop)
            {
            }

            public override string ToString()
            {
                return name + " [" + drawBox + "]";
            }

            /// <summary>
            /// Aplica um efeito de fade in
            /// </summary>
            /// <param name="time">Tempodo fading</param>
            public void FadeIn(float time)
            {
                fading = true;
                fadingIn = true;
                fadingTime = time;
                elapsed = 0;
            }

            /// <summary>
            /// Aplica um efeito de fade out
            /// </summary>
            /// <param name="time">Tempo do fading</param>
            public void FadeOut(float time)
            {
                fading = true;
                fadingIn = false;
                fadingTime = time;
                elapsed = 0;
            }

            /// <summary>
            /// Spawna a entidade no jogo.
            /// Este método somente pode ser executado uma única vez após a entidade ser criada.
            /// </summary>
            public virtual void Spawn()
            {
                drawCount = 0;
                disposed = false;
                lastDelta = Vector2D.NULL_VECTOR;
                solid = true;
                animations = new List<Animation>();

                // Para cada ImageList definido no array de ImageLists passados previamente pelo construtor.
                for (int i = 0; i < imageLists.Length; i++)
                {
                    ImageList imageList = imageLists[i];
                    float fps = DEFAULT_FPS;
                    int initialFrame = 0;
                    bool startVisible = true;
                    bool startOn = true;
                    bool loop = true;
                    // Chama o evento OnCreateAnimation() passando os como parâmetros os dados da animação a ser criada.
                    // O evento OnCreateAnimation() poderá ou não redefinir os dados da animação.
                    OnCreateAnimation(i, ref imageList, ref fps, ref initialFrame, ref startVisible, ref startOn, ref loop);
                    // Cria-se a animação com os dados retornados de OnCreateAnimation().
                    animations.Add(new Animation(this, i, imageList, fps, initialFrame, startVisible, startOn, loop));
                }
            }

            /// <summary>
            /// Engine
            /// </summary>
            public FrmBomberman Engine
            {
                get
                {
                    return engine;
                }
            }

            /// <summary>
            /// Nome da entidade
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Retângulo de desenho
            /// </summary>
            public Box2D DrawBox
            {
                get
                {
                    return drawBox;
                }
            }

            /// <summary>
            /// Retângulo de colisão
            /// </summary>
            public Box2D CollisionBox
            {
                get
                {
                    return collisionBox;
                }
            }

            /// <summary>
            /// Especifica se o desenho da entidade será feito lado a lado preenchendo o retângulo de desenho
            /// </summary>
            public bool Tiled
            {
                get
                {
                    return tiled;
                }
                set
                {
                    tiled = value;
                    engine.Repaint(this);
                }
            }

            /// <summary>
            /// Especifica a opacidade da entidade, podendo ser utilizado para causar efeito de transparência
            /// </summary>
            public float Opacity
            {
                get
                {
                    return opacity;
                }
                set
                {
                    opacity = value;
                    engine.Repaint(this);
                }
            }

            /// <summary>
            /// Vetor de deslocamento da entidade desde o último tick
            /// </summary>
            public Vector2D LastDelta
            {
                get
                {
                    return lastDelta;
                }
            }

            /// <summary>
            /// Animação correspondente a um determinado índice
            /// </summary>
            /// <param name="index">Índice da animação</param>
            /// <returns>Animação de índice index</returns>
            public Animation GetAnimation(int index)
            {
                return animations[index];
            }

            /// <summary>
            /// Evento interno que ocorrerá sempre que o efeito de fade in for completo
            /// </summary>
            protected virtual void OnFadeInComplete()
            {
            }

            /// <summary>
            /// Evento interno que ocorrerá sempre que o efeito de fade out for completo
            /// </summary>
            protected virtual void OnFadeOutComplete()
            {
            }

            /// <summary>
            /// Evento que ocorrerá uma vez a cada frame (tick) do engine
            /// </summary>
            public virtual void OnFrame()
            {
                // Verifica se está ocorrendo o fading
                if (fading)
                {
                    elapsed += TICK; // Atualiza o tempo decorrido desde o início do fading
                    Opacity = fadingIn ? fadingTime - elapsed / fadingTime : elapsed / fadingTime; // Atualiza a opacidade com base no tempo decorrido do fading (tempo inicial = opacidade 1, tempo final = opacidade 0)

                    if (elapsed >= fadingTime) // Verifica se o tempo decorrido atingiu o tempo do fading
                    {
                        fading = false;

                        // Dispara os eventos de completamento de fading
                        if (fadingIn)
                            OnFadeInComplete();
                        else
                            OnFadeOutComplete();
                    }
                }

                // Processa cada animação
                foreach (Animation animation in animations)
                    animation.OnFrame();
            }

            /// <summary>
            /// Faz a repintura da entidade
            /// </summary>
            /// <param name="g">Objeto do tipo Graphics que provém as operações de desenho</param>
            public virtual void Paint(Graphics g)
            {
                // Se este objeto já foi disposto (todos seus recursos foram liberados) enão não há nada o que fazer por aqui
                if (disposed)
                    return;

                // Realiza a repintura de cada animação
                foreach (Animation animation in animations)
                    animation.Paint(g);

                drawCount++; // Incrementa o número de desenhos feitos nesta entidade

                if (DEBUG_SHOW_ENTITY_DRAW_COUNT)
                {
                    Font font = new Font("Arial", 16 * engine.drawScale);
                    using (Brush brush = new SolidBrush(Color.Yellow))
                    {
                        Vector2D mins = drawBox.Origin + drawBox.Mins;
                        string text = drawCount.ToString();
                        SizeF size = g.MeasureString(text, font);

                        g.DrawString(text, font, brush, engine.drawOrigin.X + (mins.X + (drawBox.Width - size.Width / engine.drawScale) / 2) * engine.drawScale, engine.drawOrigin.Y + (mins.Y + (drawBox.Height - size.Height / engine.drawScale) / 2) * engine.drawScale);
                    }
                }
            }

            /// <summary>
            /// Evento interno que ocorrerá sempre que uma animação chegar ao fim
            /// </summary>
            /// <param name="animation"></param>
            internal virtual void OnAnimationEnd(Animation animation)
            {
            }

            public virtual void Read(BinaryReader reader)
            {
                
            }

            public virtual void Write(BinaryWriter writer)
            {
                
            }
        }

        /// <summary>
        /// Partição da área de desenho do jogo.
        /// Usada para dispor as entidades de forma a acelerar a busca de uma determinada entidade na tela de acordo com um retângulo de desenho especificado.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade (deve descender da classe Entity)</typeparam>
        private class Partition<T> where T : Entity
        {
            /// <summary>
            /// Elemento/Célula de uma partição.
            /// A partição é dividida em uma matriz bidimensional de células onde cada uma delas são retângulos iguais.
            /// Cada célula armazena uma lista de entidades que possuem intersecção não vazia com ela, facilitando assim a busca por entidades que possuem intersecção não vazia com um retângulo dado.
            /// </summary>
            /// <typeparam name="U">Tipo da entidade (deve descender da classe Entity)</typeparam>
            private class PartitionCell<U> where U : Entity
            {
                Partition<U> partition; // Partição a qual esta célula pertence
                Box2D box; // Retângulo que delimita a célula
                List<U> values; // Lista de entides que possuem intersecção não vazia com esta célula

                /// <summary>
                /// Cria uma nova célula para a partição
                /// </summary>
                /// <param name="partition">Partição a qual esta célula pertence</param>
                /// <param name="box">Retângulo que delimita esta célula</param>
                public PartitionCell(Partition<U> partition, Box2D box)
                {
                    this.partition = partition;
                    this.box = box;

                    values = new List<U>();
                }

                /// <summary>
                /// Insere uma nova entidade nesta célula
                /// </summary>
                /// <param name="value">Entidade a ser adicionada</param>
                public void Insert(U value)
                {
                    if (!values.Contains(value))
                        values.Add(value);
                }

                /// <summary>
                /// Obtém a lista de entidades desta célula que possui intersecção não vazia com um retângulo dado
                /// </summary>
                /// <param name="box">Retângulo usado para pesquisa</param>
                /// <param name="result">Lista de resultados a ser obtido</param>
                public void Query(Box2D box, List<U> result)
                {
                    // Verifica a lista de entidades da célula
                    foreach (U value in values)
                    {
                        Box2D intersection = value.DrawBox & box; // Calcula a intersecção do retângulo de desenho da entidade com o retângulo de pesquisa

                        if (intersection.Area() != 0 && !result.Contains(value)) // Se a intersecção for não vazia e se a entidade ainda não estiver na lista de resultados
                            result.Add(value); // adiciona esta entidade à lista
                    }
                }

                /// <summary>
                /// Atualiza uma entidade com relaçõa a esta célula, se necessário adicionando-a ou removendo-a da célula
                /// </summary>
                /// <param name="value">Entidade a ser atualizada nesta célula</param>
                public void Update(U value)
                {
                    Box2D intersection = value.DrawBox & box; // Calcula a interecção
                    bool intersectionNull = intersection.Area() == 0;

                    if (!intersectionNull && !values.Contains(value)) // Se a intersecção for não vazia e a célula ainda não contém esta entidade
                        values.Add(value); // então adiciona-a em sua lista de entidades
                    else if (intersectionNull && values.Contains(value)) // Senão, se a intesecção for vazia e esta entidade ainda está contida neta célula
                        values.Remove(value); // remove-a da sua lista de entidades
                }

                /// <summary>
                /// Remove uma entidade desta célula
                /// </summary>
                /// <param name="value">Entidade a ser removida</param>
                public void Remove(U value)
                {
                    values.Remove(value);
                }

                /// <summary>
                /// Limpa a lista de entidades desta célula
                /// </summary>
                public void Clear()
                {
                    values.Clear();
                }

                /// <summary>
                /// Obtém a quantidade de entidades que possuem intersecção não vazia com esta célula
                /// </summary>
                public int Count
                {
                    get
                    {
                        return values.Count;
                    }
                }
            }

            private Box2D box; // Retângulo que define esta partição
            private int rows; // Número de linhas da subdivisão
            private int cols; // Número de colunas da subdivisão

            private PartitionCell<T>[,] cells; // Matriz da partição
            private float cellWidth; // Largura de cada subdivisão
            private float cellHeight; // Altura de cada subdivisão

            /// <summary>
            /// Cria uma nova partição
            /// </summary>
            /// <param name="left">Coordenada x do topo superior esquerdo da partição</param>
            /// <param name="top">Coordenada y do topo superior esquerdo da partição</param>
            /// <param name="width">Largura da partição</param>
            /// <param name="height">Altura da partição</param>
            /// <param name="rows">Número de linhas da subdivisão da partição</param>
            /// <param name="cols">Número de colunas da subdivisão da partição</param>
            public Partition(float left, float top, float width, float height, int rows, int cols)
            : this(new Box2D(new Vector2D(left, top), Vector2D.NULL_VECTOR, new Vector2D(width, height)), rows, cols)
            {
            }

            /// <summary>
            /// Cria uma nova partição
            /// </summary>
            /// <param name="rect">Retângulo que delimita a partição</param>
            /// <param name="rows">Número de linhas da subdivisão da partição</param>
            /// <param name="cols">Número de colunas da subdivisão da partição</param>
            public Partition(Rectangle rect, int rows, int cols)
            : this(new Box2D(rect), rows, cols)
            {
            }

            /// <summary>
            /// Cria uma nova partição
            /// </summary>
            /// <param name="rect">Retângulo que delimita a partição</param>
            /// <param name="rows">Número de linhas da subdivisão da partição</param>
            /// <param name="cols">Número de colunas da subdivisão da partição</param>
            public Partition(RectangleF rect, int rows, int cols)
            : this(new Box2D(rect), rows, cols)
            {
            }

            /// <summary>
            /// Cria uma nova partição
            /// </summary>
            /// <param name="box">Retângulo que delimita a partição</param>
            /// <param name="rows">Número de linhas da subdivisão da partição</param>
            /// <param name="cols">Número de colunas da subdivisão da partição</param>
            public Partition(Box2D box, int rows, int cols)
            {
                this.box = box;
                this.rows = rows;
                this.cols = cols;

                cellWidth = box.Width / cols; // Calcula a largura de cada subdivisão
                cellHeight = box.Height / rows; // Calcula a altura de cada subdivisão

                cells = new PartitionCell<T>[cols, rows]; // Cria a matriz de subdivisões
            }

            /// <summary>
            /// Insere uma nova entidade a partição
            /// </summary>
            /// <param name="item">Entidade a ser adicionada</param>
            public void Insert(T item)
            {
                Box2D box = item.DrawBox;

                // Calcula os mínimos e máximos absolutos do retângulo que delimita esta partição
                Vector2D origin = this.box.Origin;
                Vector2D mins = this.box.Mins + origin;
                Vector2D maxs = this.box.Maxs + origin;

                // Calcula os mínimos e máximos absolutos do retângulo de desenho da entidade a ser adicionada
                Vector2D origin1 = box.Origin;
                Vector2D mins1 = box.Mins + origin1;
                Vector2D maxs1 = box.Maxs + origin1;

                int startCol = (int) ((mins1.X - mins.X) / cellWidth); // Calcula a coluna da primeira célula a qual interceptará a entidade
                int startRow = (int) ((mins1.Y - mins.Y) / cellHeight); // Calcula a primeira linha da primeira célula a qual interceptará a entidade

                int endCol = (int) ((maxs1.X - mins.X - 1) / cellWidth); // Calcula a coluna da última célula a qual interceptará a entidade

                if (endCol >= cols)
                    endCol = cols - 1;

                int endRow = (int) ((maxs1.Y - mins.Y - 1) / cellHeight); // Calcula a linha da última célula a qual intercepetará a entidade

                if (endRow >= rows)
                    endRow = rows - 1;

                // Varre todas as possíveis células que podem interceptar a entidade dada
                for (int i = startCol; i <= endCol; i++)
                    for (int j = startRow; j <= endRow; j++)
                    {
                        Box2D box1 = new Box2D(new Vector2D(mins.X + cellWidth * i, mins.Y + cellHeight * j), Vector2D.NULL_VECTOR, new Vector2D(cellWidth, cellHeight));
                        Box2D intersection = box1 & box; // Calcula a intesecção

                        if (intersection.Area() == 0) // Se a intesecção for vazia, não precisa adicionar a entidade a célula
                            continue;

                        if (cells[i, j] == null) // Verifica se a célula já foi criada antes, caso não tenha sido ainda então a cria
                            cells[i, j] = new PartitionCell<T>(this, box1);

                        cells[i, j].Insert(item); // Insere a entidade na célula
                    }
            }

            /// <summary>
            /// Realiza uma busca de quais entidades possuem intesecção não vazia com um retângulo dado
            /// </summary>
            /// <param name="box"></param>
            /// <returns></returns>
            public List<T> Query(Box2D box)
            {
                List<T> result = new List<T>();

                // Calcula os máximos e mínimos absulutos do retângulo que delimita esta partição
                Vector2D origin = this.box.Origin;
                Vector2D mins = this.box.Mins + origin;
                Vector2D maxs = this.box.Maxs + origin;

                // Calcula os máximos e mínimos do retângulo de pesquisa
                Vector2D origin1 = box.Origin;
                Vector2D mins1 = box.Mins + origin1;
                Vector2D maxs1 = box.Maxs + origin1;

                int startCol = (int) ((mins1.X - mins.X) / cellWidth); // Calcula a coluna da primeira célula a qual deverá ser consultada

                if (startCol < 0)
                    startCol = 0;

                int startRow = (int) ((mins1.Y - mins.Y) / cellHeight); // Calcula a primeira linha da primeira célula a qual deverá ser consultada

                if (startRow < 0)
                    startRow = 0;

                int endCol = (int) ((maxs1.X - mins.X - 1) / cellWidth); // Calcula a colna da última célula a qual deverá ser consultada

                if (endCol >= cols)
                    endCol = cols - 1;

                int endRow = (int) ((maxs1.Y - mins.Y - 1) / cellHeight); // Calcula a linha da última célula a qual deverá ser consultada

                if (endRow >= rows)
                    endRow = rows - 1;

                // Varre todas as possíveis células que poderão ter intersecção não vazia com o retângulo dado
                for (int i = startCol; i <= endCol; i++)
                    for (int j = startRow; j <= endRow; j++)
                        if (cells[i, j] != null) // Para cada célula que já foi previamente criada
                            cells[i, j].Query(box, result); // consulta quais entidades possuem intersecção não vazia com o retângulo dado

                return result;
            }

            /// <summary>
            /// Atualiza uma entidade nesta partição.
            /// Este método deve ser chamado sempre que a entidade tiver sua posição ou dimensões alteradas.
            /// </summary>
            /// <param name="item">Entidade a ser atualizada dentro da partição</param>
            public void Update(T item)
            {
                Vector2D delta = item.LastDelta; // Obtém o vetor de deslocamento da entidade desde o último tick

                if (delta == Vector2D.NULL_VECTOR) // Se a entidade não se deslocou desde o último tick então não há nada o que se fazer aqui
                    return;

                Box2D box = item.DrawBox; // Obtém o retângulo de desenho atual da entidade
                Box2D box0 = box - delta; // Obtém o retângulo de desenho da entidade antes do deslocamento (do tick anterior)

                // Calcula os máximos e mínimos absolutos do retângulo que delimita esta partição
                Vector2D origin = this.box.Origin;
                Vector2D mins = this.box.Mins + origin;
                Vector2D maxs = this.box.Maxs + origin;

                // Calcula os máximos e mínimos absolutos do rêtângulo de desenho anterior da entidade
                Vector2D origin0 = box0.Origin;
                Vector2D mins0 = box0.Mins + origin0;
                Vector2D maxs0 = box0.Maxs + origin0;

                // Calcula os máximos e mínimos absolutos do retângulo de desenho atual da entidade
                Vector2D origin1 = box.Origin;
                Vector2D mins1 = box.Mins + origin1;
                Vector2D maxs1 = box.Maxs + origin1;

                int startCol = (int) ((Math.Min(mins0.X, mins1.X) - mins.X) / cellWidth); // Calcula a coluna da primeira célula para qual deverá ser verificada
                int startRow = (int) ((Math.Min(mins0.Y, mins1.Y) - mins.Y) / cellHeight); // Calcula a linha da primeira célula para a qual deverá ser verificada

                int endCol = (int) ((Math.Max(maxs0.X, maxs1.X) - mins.X - 1) / cellWidth); // Calcula a coluna da útlima célula para qual deverá ser verificada

                if (endCol >= cols)
                    endCol = cols - 1;

                int endRow = (int) ((Math.Max(maxs0.Y, maxs1.Y) - mins.Y - 1) / cellHeight); // Calcula a linha da última célula para qual deverá ser verificada

                if (endRow >= rows)
                    endRow = rows - 1;

                // Varre todas as possíveis células que possui ou possuiam intersecção não vazia com a entidade dada
                for (int i = startCol; i <= endCol; i++)
                    for (int j = startRow; j <= endRow; j++)
                        if (cells[i, j] != null) // Se a célula já existir
                        {
                            cells[i, j].Update(item); // Atualiza a entidade dentro da célula

                            if (cells[i, j].Count == 0) // Se a célula não possuir mais entidades, defina como nula
                                cells[i, j] = null;
                        }
                        else
                        {
                            // Senão...
                            Box2D box1 = new Box2D(new Vector2D(mins.X + cellWidth * i, mins.Y + cellHeight * j), Vector2D.NULL_VECTOR, new Vector2D(cellWidth, cellHeight));
                            Box2D intersection = box1 & box; // Calcula a intersecção desta célula com o retângulo de desenho atual da entidade

                            if (intersection.Area() == 0) // Se ela for vazia, não há nada o que fazer nesta célula
                                continue;

                            // Senão...
                            if (cells[i, j] == null) // Verifica se a célula é nula
                                cells[i, j] = new PartitionCell<T>(this, box1); // Se for, cria uma nova célula nesta posição

                            cells[i, j].Insert(item); // e finalmente insere a entidade nesta célula
                        }
            }

            /// <summary>
            /// Remove uma entidade da partição
            /// </summary>
            /// <param name="item">Entidade a ser removida</param>
            public void Remove(T item)
            {
                Box2D box = item.DrawBox; // Obtém o retângulo de desenho da entidade

                // Calcula os máximos e mínimos absolutos do retângulo que delimita esta partição
                Vector2D origin = this.box.Origin;
                Vector2D mins = this.box.Mins + origin;
                Vector2D maxs = this.box.Maxs + origin;

                // Calcula os máximos e mínimos absolutos do retângulo de desenho da entidade a ser removida
                Vector2D origin1 = box.Origin;
                Vector2D mins1 = box.Mins + origin1;
                Vector2D maxs1 = box.Maxs + origin1;

                int startCol = (int) ((mins1.X - mins.X) / cellWidth); // Calcula a coluna da primeira célula a ser verificada
                int startRow = (int) ((mins1.Y - mins.Y) / cellHeight); // Calcula a linha da primeira célula a ser verificada

                int endCol = (int) ((maxs1.X - mins.X - 1) / cellWidth); // Calcula a coluna da última célula a ser verificada

                if (endCol >= cols)
                    endCol = cols - 1;

                int endRow = (int) ((maxs1.Y - mins.Y - 1) / cellHeight); // Calcula a linha da última célula a ser verificada

                if (endRow >= rows)
                    endRow = rows - 1;

                // Varre todas as possíveis células que podem ter intersecção não vazia com a entidade dada
                for (int i = startCol; i <= endCol; i++)
                    for (int j = startRow; j <= endRow; j++)
                        if (cells[i, j] != null)
                        {
                            cells[i, j].Remove(item); // Remove a entidade da célula caso ela possua intersecção não vazia com a célula

                            if (cells[i, j].Count == 0) // Se a célula não possuir mais entidades
                                cells[i, j] = null; // defina-a como nula
                        }
            }

            /// <summary>
            /// Exclui todas as entidades contidas na partição
            /// </summary>
            public void Clear()
            {
                for (int i = 0; i < cols; i++)
                    for (int j = 0; j < rows; j++)
                        if (cells[i, j] != null)
                        {
                            cells[i, j].Clear();
                            cells[i, j] = null;
                        }
            }
        }

        /// <summary>
        /// Bloco não destrutível
        /// </summary>
        public class HardBlock : Entity
        {
            private int skin; // Skin usada pelo bloco

            /// <summary>
            /// Cria um novo bloco não destrutível
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do bloco</param>
            /// <param name="box">Retângulo delimitador do bloco</param>
            /// <param name="imageLists">Lista de imagens contendo todas as skins do bloco</param>
            /// <param name="skin">Número da skin a ser usada pelo bloco</param>
            public HardBlock(FrmBomberman engine, string name, Box2D box, ImageList[] imageLists, int skin)
            : base(engine, name, box, box, imageLists, true)
            {
                this.skin = skin;
            }

            protected override void OnCreateAnimation(int animationIndex, ref ImageList imageList, ref float fps, ref int initialFrame, ref bool startVisible, ref bool startOn, ref bool loop)
            {
                // Sempre que uma animação for criada
                base.OnCreateAnimation(animationIndex, ref imageList, ref fps, ref initialFrame, ref startVisible, ref startOn, ref loop);
                startOn = false; // ela não deverá ser executada
                startVisible = true; // mas deverá ser visívei
            }

            /// <summary>
            /// Skin usada pelo bloco
            /// </summary>
            public int Skin
            {
                get
                {
                    return skin;
                }
                set
                {
                    skin = value;
                    animations[0].CurrentFrame = value; // O quadro atual da animação será o número da skin, lembrando que a animação não avançará quadro por quadro mas apenas exibirá o quadro atual.
                }
            }

            public override void Spawn()
            {
                base.Spawn();

                animations[0].CurrentFrame = skin; // Define o quadro atual da animação para a skin inicial deste bloco
            }
        }

        /// <summary>
        /// Um sprite é um elemento móvel dentro do jogo, além de posição ele também possui velocidade e pode interagir fisicamente com outros elementos do jogo sendo eles blocos estáticos ou outros sprites.
        /// </summary>
        public abstract class Sprite : Entity
        {
            private int index; // Posição deste sprite na lista de sprites do engine
            private Box2D hitBox; // Retângulo que delimita a área de dano deste sprite

            protected Vector2D vel; // Velocidade
            protected bool moving; // Indica se o sprite está continuou se movendo desde a última iteração física com os demais elementos do jogo
            protected bool markedToRemove; // Indica se o sprite foi marcado para remoção, tal remoção só será realizada após ser executada todas as interações físicas entre os elementos do jogo.
            protected bool isStatic; // Indica se o sprite é estático
            protected bool breakable; // Indica se ele pode ser quebrado
            protected bool passable; // Indica se ele pode ser atravessado por outras entidades
            protected bool passStatics; // Indica se ele pode atravessar entidades estáticas
            protected float health; // HP do sprite
            protected float maxDamage; // Dano máximo que o sprite poderá receber de uma só vez
            protected bool invincible; // Indica se o sprite está invencível, não podendo assim sofrer danos
            protected float invincibilityTime; // Indica o tempo de invencibilidade do sprite quando ele estiver invencível
            private float invincibleExpires; // Indica o instante no qual a invencibilidade do sprite irá terminar. Tal instante é dado em segundos e é relativo ao tempo de execução do engine.
            private List<Sprite> touchingSprites; // Lista todos os outros sprites que estão tocando este sprite (que possuem intersecção não vazia entre seus retângulos de colisão)
            protected bool broke; // Indica se este sprite foi quebrado

            /// <summary>
            /// Cria um novo sprite
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do sprite</param>
            /// <param name="box">Retângulo que delimitará a área de desenho, de dano e de colisão do novo sprite</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do sprite</param>
            /// <param name="tiled">Indicará se o desenho do sprite será feito preenchendo-se lado a lado o retângulo de desenho</param>
            public Sprite(FrmBomberman engine, string name, Box2D box, ImageList[] imageLists, bool tiled = false)
            : this(engine, name, box, box, box, imageLists, tiled)
            {
            }

            /// <summary>
            /// Cria um novo sprite
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do sprite</param>
            /// <param name="drawBox">Retângulo que delimitará a área de desenho do novo sprite</param>
            /// <param name="collisionBox">Retângulo que delimitará a área de dano e de colisão do novo sprite</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do sprite</param>
            /// <param name="tiled">Indicará se o desenho do sprite será feito preenchendo-se lado a lado o retângulo de desenho</param>
            public Sprite(FrmBomberman engine, string name, Box2D drawBox, Box2D collisionBox, ImageList[] imageLists, bool tiled = false)
            : this(engine, name, drawBox, collisionBox, collisionBox, imageLists, tiled)
            {
            }

            /// <summary>
            /// Cria um novo sprite
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do sprite</param>
            /// <param name="drawBox">Retângulo que delimitará a área de desenho do novo sprite</param>
            /// <param name="drawBox">Retângulo que delimitará a área de dano do novo sprite</param>
            /// <param name="collisionBox">Retângulo que delimitará a área de colisão do novo sprite</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do sprite</param>
            /// <param name="tiled">Indicará se o desenho do sprite será feito preenchendo-se lado a lado o retângulo de desenho</param>
            public Sprite(FrmBomberman engine, string name, Box2D drawBox, Box2D hitBox, Box2D collisionBox, ImageList[] imageLists, bool tiled = false)
            : base(engine, name, drawBox, collisionBox, imageLists, tiled)
            {
                this.hitBox = hitBox;
            }

            /// <summary>
            /// Posição deste sprite na lista de sprites do engine
            /// </summary>
            public int Index
            {
                get
                {
                    return index;
                }
            }

            /// <summary>
            /// Retângulo que delimita a área de dano deste sprite. Normalmente ele está contido dentro do retângulo de colisão.
            /// </summary>
            public Box2D HitBox
            {
                get
                {
                    return hitBox;
                }
            }

            /// <summary>
            /// Indica se este sprite é estático
            /// </summary>
            public bool Static
            {
                get
                {
                    return isStatic;
                }
            }

            /// <summary>
            /// Indica se este sprite pode atravessar sprites estáticos
            /// </summary>
            public bool PassStatics
            {
                get
                {
                    return passStatics;
                }
                set
                {
                    passStatics = value;
                }
            }

            /// <summary>
            /// Indica se este sprite ainda está se movendo desde a última interação física com os demais sprites do jogo
            /// </summary>
            public bool Moving
            {
                get
                {
                    return moving;
                }
            }

            /// <summary>
            /// Indica se este sprite foi quebrado
            /// </summary>
            public bool Broke
            {
                get
                {
                    return broke;
                }
            }

            /// <summary>
            /// Indica se este sprite foi marcado para remoção. Tal remoção só ocorrerá depois de serem processadas todas as interações físicas entre os elementos do jogo.
            /// </summary>
            public bool MarkedToRemove
            {
                get
                {
                    return markedToRemove;
                }
            }

            /// <summary>
            /// Indica se este sprite está no modo de invencibilidade
            /// </summary>
            public bool Invincible
            {
                get
                {
                    return invincible;
                }
            }

            /// <summary>
            /// HP deste sprite
            /// </summary>
            public float Health
            {
                get
                {
                    return health;
                }
                set
                {
                    health = value;
                    OnHealthChanged(health); // Lança o evento notificando a mudança do HP

                    if (health == 0) // Se o HP for zero
                        Break(null); // quebre-a!
                }
            }

            /// <summary>
            /// Dano máximo que este sprite poderá receber de uma só vez
            /// </summary>
            public float MaxDamage
            {
                get
                {
                    return maxDamage;
                }
                set
                {
                    maxDamage = value;
                }
            }

            /// <summary>
            /// Vetor velocidade deste sprite
            /// </summary>
            public Vector2D Velocity
            {
                get
                {
                    return vel;
                }
                set
                {
                    vel = value;
                }
            }

            /// <summary>
            /// Mata o sprite (sem quebra-la).
            /// </summary>
            public void Kill(Bomberman killer)
            {
                OnDeath(killer);
            }

            /// <summary>
            /// Libera todos os recursos associados a este sprite
            /// </summary>
            public override void Dispose()
            {
                markedToRemove = true;
                engine.removedSprites.Add(this);
                base.Dispose();
                engine.Repaint(this);
            }

            /// <summary>
            /// Evento interno que é lançado sempre que o sprite for morto
            /// </summary>
            protected virtual void OnDeath(Bomberman killer)
            {
                Dispose(); // Por padrão ele apenas dispões ele da memória, liberando todos os recursos associados a ele
            }

            /// <summary>
            /// Teleporta o sprite para uma determinada posição
            /// </summary>
            /// <param name="pos">Nova posição onde o sprite irá se localizar</param>
            public void TeleportTo(Vector2D pos)
            {
                Vector2D delta = pos - collisionBox.Origin; // Obtém o vetor de deslocamento
                                                            // Atualiza a posição de todos os retângulos
                drawBox += delta; // de desenho
                collisionBox += delta; // de colisão
                hitBox += delta; // e de dano
                lastDelta += delta; // E atualiza também o vetor de deslocamento absoluto (que indica o quanto ele se deslocou desde o último tick do engine)

                // Atualiza as partições do engine
                if (isStatic)
                    engine.partitionStatics.Update(this);
                else
                    engine.partitionSprites.Update(this);

                engine.Repaint(this); // Notifica o engine que este sprite deverá ser redesenhado
            }

            /// <summary>
            /// Realiza o spawn do sprite. Este método deverá ser chamado apenas uma vez desde a criação do sprite.
            /// </summary>
            public override void Spawn()
            {
                base.Spawn(); // Chama o método base

                // Inicializa todos os campos
                vel = new Vector2D();
                moving = false;
                markedToRemove = false;
                isStatic = false;
                breakable = true;
                passable = false;
                passStatics = false;
                health = DEFAULT_HEALTH;
                maxDamage = DEFAULT_MAX_DAMAGE;
                invincible = false;
                invincibilityTime = DEFAULT_INVINCIBLE_TIME;
                touchingSprites = new List<Sprite>();
                broke = false;

                engine.addedSprites.Add(this); // Adiciona este sprite a lista de sprites do engine
            }

            /// <summary>
            /// Evento interno que será chamado sempre que o sprite estiver a sofrer um dano.
            /// Classes descententes a esta poderão sobrepor este método para definir o comportamento do dano ou até mesmo cancelá-lo antes mesmo que ele seja processado.
            /// </summary>
            /// <param name="attacker">Atacante, o sprite que irá causar o dano</param>
            /// <param name="region">Retângulo que delimita a área de dano a ser infringida neste sprite pelo atacante</param>
            /// <param name="damage">Quandidade de dano a ser causada pelo atacante. É passado por referência e portanto qualquer alteração deste parâmetro poderá mudar o comportamento do dano sofrido por este sprite.</param>
            /// <returns>true se o dano deverá ser processado, false se o dano deverá ser cancelado</returns>
            protected virtual bool OnTakeDamage(Sprite attacker, Box2D region, ref float damage)
            {
                return true;
            }

            /// <summary>
            /// Evento interno que será chamado sempre que o sprite sofreu um dano.
            /// </summary>
            /// <param name="attacker">Atacante, o sprite que causou o dano</param>
            /// <param name="region">Retângulo que delimita a área de dano infringido neste sprite pelo atacante</param>
            /// <param name="damage">Quantidade de dano causada pelo atacante</param>
            protected virtual void OnTakeDamagePost(Sprite attacker, Box2D region, float damage)
            {
            }

            /// <summary>
            /// Evento interno que será chamado sempre que o HP deste sprite for alterado
            /// </summary>
            /// <param name="health"></param>
            protected virtual void OnHealthChanged(float health)
            {
            }

            /// <summary>
            /// Causa um dano em uma vítima
            /// A área de dano será causada usando o retângulo de colisão do atacante, normalmente o dano só é causado quando os dois estão colidindo, ou melhor dizendo, quando a intersecção do retângulo de colisão do atacante e o retângulo de dano da vítima for não vazia.
            /// </summary>
            /// <param name="victim">Vítima que sofrerá o dano/param>
            /// <param name="damage">Quantidade de dano a ser causada na vítima</param>
            public void Hurt(Sprite victim, Bomberman attacker, float damage)
            {
                Hurt(victim, attacker, collisionBox, damage);
            }

            /// <summary>
            /// Causa um dano numa determinada região de uma vítima
            /// </summary>
            /// <param name="victim">Vítima que sofrerá o dano</param>
            /// <param name="region">Retângulo delimitando a região no qual o dano será aplicado na vítima. Norlammente o dano só é aplicado quando a interseção deste retângulo com o retângulo de dano da vítima for não vazia.</param>
            /// <param name="damage">Quantidade de dano a ser causada na vítima</param>
            public void Hurt(Sprite victim, Bomberman attacker, Box2D region, float damage)
            {
                // Se a vítima já estver quebrada, se estiver marcada para remoção ou seu HP não for maior que zero então não há nada o que se fazer aqui.
                if (victim.broke || victim.markedToRemove || health <= 0)
                    return;

                Box2D intersection = victim.hitBox & region; // Calcula a intesecção com a área de dano da vítima e a região dada

                if (intersection.Area() == 0) // Se a intersecção for vazia, não aplica o dano
                    return;

                if (damage > maxDamage) // Verifica se o dano aplicado é maior que o máximo de dano permitido pela vítima
                    damage = maxDamage; // Se for, trunca o dano a ser aplicado

                // Verifica se a vítima não está em modo de invencibilidade e se seu evento OnTakeDamage indica que o dano não deverá ser aplicado
                if (!victim.invincible && victim.OnTakeDamage(this, region, ref damage))
                {
                    // Lembrando também que a chamada ao evento OnTakeDamage pode alterar a quantidade de dano a ser aplicada na vítima
                    float h = victim.health; // Obtém o HP da vítima
                    h -= damage; // Subtrai o HP da vítima pela quantidade de dano a ser aplicada

                    if (h < 0) // Verifica se o resultado é negativo
                        h = 0; // Se for, o HP deverá então ser zero

                    victim.health = h; // Define o HP da vítima com este novo resultado
                    victim.OnHealthChanged(h); // Notifica a vítima de que seu HP foi alterado
                    victim.OnTakeDamagePost(this, region, damage); // Notifica a vítima de que um dano foi causado

                    if (victim.health == 0) // Verifica se o novo HP da vítima é zero
                        victim.Break(attacker); // Se for, quebre-a!
                    else
                        victim.MakeInvincible(); // Senão, aplica a invencibilidade temporária após sofrer o dano
                }
            }

            /// <summary>
            /// Torna este sprite invencível por um determinado período de tempo em segundos
            /// </summary>
            /// <param name="time">Se for positivo, representará o tempo em segundos no qual este sprite ficará invencível, senão será aplicada a invencibilidade usando o tempo de invencibilidade padrão da vítima.</param>
            public void MakeInvincible(float time = 0)
            {
                invincible = true; // Marca o sprite como invencível
                invincibleExpires = engine.GetEngineTime() + (time <= 0 ? invincibilityTime : time); // Calcula o tempo em que a invencibilidade irá acabar

                // Aplica o efeito de pisca pisca em todas as animações deste sprite
                foreach (Animation animation in animations)
                    animation.Flashing = true;
            }

            /// <summary>
            /// Evento que será chamado sempre que este sprite for adicionado na lista de sprites do engine
            /// </summary>
            /// <param name="index">Posição deste sprite na lista de sprites do engine</param>
            public void OnAdded(int index)
            {
                this.index = index;
            }

            /// <summary>
            /// Evento interno que será chamdo sempre que for checada a colisão deste sprite com outro sprite.
            /// Sobreponha este método em classes bases para alterar o comportamento deste sprite com relação a outro sempre que estiverem a colidir.
            /// </summary>
            /// <param name="sprite">Sprite a ser verificado</param>
            /// <returns>true se os dois sprites deverão colidor, false caso contrário. Como padrão este método sempre retornará false indicando que os dois sprites irão colidir</returns>
            protected virtual bool ShouldCollide(Sprite sprite)
            {
                return false;
            }

            /// <summary>
            /// Verifica a colisão deste sprite com os blocos (ou também com qualquer outro sprite marcada como estático)
            /// </summary>
            /// <returns>Vetor de deslocamento deste sprite após verificada as possíveis colisões</returns>
            private Vector2D CheckCollisionWithStatics()
            {
                Vector2D velRot = vel.Versor().Rotate90(); // Calcula o versor da velocidade, rotacioa-o em 90 graus.

                // Usa-se a medida do comprimento das esquinas para verifica se é possível o sprite tomar uma nova rota sem colidir. Isso permite a suavização do movimento do sprite (principalmente do bomberman) ao virar esquinas.
                for (int dx = 0; dx <= 2 * CORNER_SIZE + 1; dx++)
                {
                    Box2D newBox = collisionBox + (dx > CORNER_SIZE ? CORNER_SIZE - dx : dx) * velRot + TICK * vel; // Calcula o novo retângulo de colisão deslocando-se o retângulo de colisão atual na direção do vetor velocidade escalado pelo tick do engine (intervalo entre cada frame).
                    Box2D union = Box2D.EMPTY_BOX;

                    // Verifica a colisão para cada bloco não quebrável
                    foreach (HardBlock blok in engine.blocks)
                    {
                        Box2D oldIntersection = collisionBox & blok.CollisionBox; // Calcula a intesecção do retângulo de colisão anterior deste sprite com o retângulo de colisão do bloco

                        if (oldIntersection.Area() != 0) // Se a intersecção for não vazia, não processe colisão
                            continue;

                        Box2D intersection = newBox & blok.CollisionBox;

                        if (intersection.Area() != 0) // Apenas deverá ser processada a colisão se anteriormente o bloco não estava colidindo com este sprite
                            union = union | intersection;
                    }

                    // Se este sprite não atravessa sprites estáticos como por exemplo os blocos quebráveis (soft blocks), verifica a colisão com todos os sprites marcados como estáticos
                    if (!passStatics)
                        foreach (Sprite sprite in engine.sprites)
                        {
                            // Se pra cada sprite da lista de sprites do engine, este sprite for eu mesmo, estiver marcado para remoção ou ele não for estático, não processe nada por enquanto aqui
                            if (sprite == this || sprite.markedToRemove || !sprite.isStatic)
                                continue;

                            Box2D oldIntersection = collisionBox & sprite.CollisionBox; // Calcula a intesecção do retângulo de colisão anterior deste sprite com o retângulo de colisão do outro sprite

                            if (oldIntersection.Area() != 0) // Se a intersecção for não vazia, não processe colisão
                                continue;

                            Box2D intersection = newBox & sprite.CollisionBox;

                            if (intersection.Area() != 0) // Apenas deverá ser processada a colisão se anteriormente o outro sprite não estava colidindo com este sprite
                                union = union | intersection;
                        }

                    // Se ouve colisão, teste uma nova posição ao longo da esquina
                    if (union.Area() != 0)
                        continue;

                    // Senão
                    return newBox.Origin - collisionBox.Origin; // calcula o vetor de deslocamento e retorne-o
                }

                return Vector2D.NULL_VECTOR; // Se ouve colisão, retorne o vetor nulo
            }

            /// <summary>
            /// Verifica a colisão com os sprites (que não estejam marcados como estáticos)
            /// </summary>
            /// <param name="delta">Vetor de deslocamento</param>
            /// <param name="touching">Lista de sprites que estarão tocando este sprite, usada como retorno</param>
            /// <returns>Um novo vetor de deslocamento</returns>
            private Vector2D CheckCollisionWithSprites(Vector2D delta, List<Sprite> touching)
            {
                Box2D newBox = collisionBox + delta; // Calcula o vetor de deslocamento inicial
                Vector2D result = delta;

                // Para cada sprite do engine
                for (int i = 0; i < engine.sprites.Count; i++)
                {
                    Sprite sprite = engine.sprites[i];

                    // Se ele for eu mesmo, se estiver marcado para remoção ou se ele for estático, não processe nada aqui
                    if (sprite == this || sprite.markedToRemove || sprite.isStatic)
                        continue;

                    Box2D oldIntersection = collisionBox & sprite.CollisionBox; // Calcula a intersecção do retângulo de colisão anterior deste sprite com o do outro sprite

                    if (oldIntersection.Area() != 0) // Se ela for não vazia
                    {
                        touching.Add(sprite); // Adiciona o outro sprite na lista de toques de retorno
                        continue; // Mas não processa colisão aqui
                    }

                    Box2D intersection = newBox & sprite.CollisionBox;

                    if (intersection.Area() != 0) // Processe colisão somente se a intersecção com o retângulo de colisão atual for não vazia
                    {
                        touching.Add(sprite); // Adicionando a lista de toques de retorno

                        if (CollisionCheck(sprite)) // E verificando se haverá colisão
                            result = Vector2D.NULL_VECTOR; // Se ouver, o novo vetor de deslocamento deverá ser nulo
                    }
                }

                return result;
            }

            /// <summary>
            /// Evento interno que será chamado sempre que este sprite começar a tocar outro sprite
            /// </summary>
            /// <param name="sprite">Sprite que começou a me tocar</param>
            protected virtual void OnStartTouch(Sprite sprite)
            {
            }

            /// <summary>
            /// Evento interno que será chamado enquanto este sprite estiver tocando outro sprite. A chamada ocorre somente uma vez a cada tick do engine enquanto os dois sprites estiverem se tocando.
            /// </summary>
            /// <param name="sprite">Sprite que está me tocando</param>
            protected virtual void OnTouching(Sprite sprite)
            {
            }

            /// <summary>
            /// Evento interno que será chamado toda vez que este este sprite parar de tocar outro sprite que estava previamente tocando este
            /// </summary>
            /// <param name="sprite">Sprite que deixou de me tocar</param>
            protected virtual void OnEndTouch(Sprite sprite)
            {
            }

            /// <summary>
            /// Realiza as interações físicas deste sprite com os demais elementos do jogo.
            /// </summary>
            private void DoPhysics()
            {
                // Verifica se ele estava se movendo no último frame mas a velocidade atual agora é nula
                if (vel.IsNull && moving)
                {
                    // Se for
                    StopMoving(); // notifica que o movimento parou
                    engine.Repaint(this); // Notifica o engine que deverá ser feita este sprite deverá ser redesenhado
                }

                Vector2D delta = !isStatic && !vel.IsNull ? CheckCollisionWithStatics() : Vector2D.NULL_VECTOR; // Verifica a colisão deste sprite com os blocos do jogo e outros sprites marcados como estáticos, retornando o vetor de deslocamento
                List<Sprite> touching = new List<Sprite>();
                delta = CheckCollisionWithSprites(delta, touching); // Verifica a colisão deste sprite com os sprites do jogo, verificando também quais sprites estão tocando este sprite e retornando o vetor de deslocamento
                lastDelta = delta; // Atualiza o vetor de deslocamento global deste sprite

                if (delta != Vector2D.NULL_VECTOR) // Se o deslocamento não for nulo
                {
                    // Translata todos os retângulos deste sprite com base no novo deslocamento
                    collisionBox += delta; // o de colisão
                    drawBox += delta; // de desenho
                    hitBox += delta; // e de dano

                    // Atualiza a partição do engine
                    if (isStatic)
                        engine.partitionStatics.Update(this);
                    else
                        engine.partitionSprites.Update(this);

                    StartMoving(); // Notifica que o sprite começou a se mover, caso ele estivesse parado antes
                    engine.Repaint(this); // Notifica o engine que este sprite deverá ser redesenhado
                }
                else if (moving) // Senão, se ele estava se movendo
                {
                    StopMoving(); // Notifica que ele parou de se mover
                    engine.Repaint(this); // Notifica o engine que este sprite deverá ser redesenhado
                }

                // Processa a lista global de sprites que anteriormente estavam tocando esta entidade no frame anterior
                int count = touchingSprites.Count;

                for (int i = 0; i < count; i++)
                {
                    // Se pra cada sprite desta lista
                    Sprite sprite = touchingSprites[i];
                    int index = touching.IndexOf(sprite);

                    if (index == -1) // ele não estiver na lista de toques local (ou seja, não está mais tocando este sprite)
                    {
                        touchingSprites.RemoveAt(i); // então remove-o da lista global
                        i--;
                        count--;
                        OnEndTouch(sprite); // e notifica que ele nao está mais tocando esta sprite
                    }
                    else // Senão
                    {
                        touching.RemoveAt(index); // Remove da lista local de toques
                        OnTouching(sprite); // Notifica que ele continua tocando este sprite
                    }
                }

                // Para cada sprites que sobrou na lista de toques local
                foreach (Sprite sprite in touching)
                {
                    touchingSprites.Add(sprite); // Adiciona-o na lista global de toques
                    OnStartTouch(sprite); // e notifique que ele começou a tocar este sprite
                }
            }

            /// <summary>
            /// Verifica se este sprite deverá colidir com outro sprite e vice versa
            /// </summary>
            /// <param name="sprite">Sprite a ser verificado</param>
            /// <returns>true se a colisão deverá ocorrer, false caso contrário</returns>
            protected bool CollisionCheck(Sprite sprite)
            {
                return (ShouldCollide(sprite) || sprite.ShouldCollide(this));
            }

            /// <summary>
            /// Evento que ocorre uma vez a cada frame (tick) do engine.
            /// Aqui ocorre a interação deste sprite com os demais elementos do jogo como tratamento de colisões, disparo de eventos de toque, inteligência artificial, etc
            /// </summary>
            public override void OnFrame()
            {
                // Se ele estiver marcado para remoção não há nada o que se fazer aqui
                if (markedToRemove)
                    return;

                // Realiza o pré-pensamento do sprite. Nesta chamada verifica-se se deveremos continuar a processar as interações deste sprite com o jogo.
                if (!PreThink())
                    return;

                // Se ele não for estático, processa as interações físicas deste sprite com os outros elementos do jogo
                if (!isStatic)
                    DoPhysics();

                // Se ele estiver invencível, continue gerando o efeito de pisca pisca
                if (invincible && engine.GetEngineTime() >= invincibleExpires)
                {
                    invincible = false;

                    foreach (Animation animation in animations)
                        animation.Flashing = false;
                }

                Think(); // Realiza o pensamento do sprite, usado para implementação de inteligência artificial

                base.OnFrame(); // Chama o evento OnFrame da classe base

                PostThink(); // Realiza o pós-pensamento do sprite
            }

            /// <summary>
            /// Inicia o movimento deste sprite
            /// </summary>
            private void StartMoving()
            {
                if (moving)
                    return;

                moving = true;
                OnStartMoving(); // Notifica que este sprite começou a se mover
            }

            /// <summary>
            /// Para o movimento deste sprite
            /// </summary>
            private void StopMoving()
            {
                if (!moving)
                    return;

                moving = false;
                OnStopMoving(); // Notifica que este sprite parou de se mover
            }

            /// <summary>
            /// Evento interno que é chamado sempre que este sprite começar a se mover
            /// </summary>
            protected virtual void OnStartMoving()
            {
            }

            /// <summary>
            /// Evento interno que é chamado sempre que este sprite parar de se mover
            /// </summary>
            protected virtual void OnStopMoving()
            {
            }

            /// <summary>
            /// Evento interno que é chamado antes de ser processada as interações físicas deste sprite com os demais elementos do jogo.
            /// Sobreponha este evento em suas casses descendentes se desejar controlar o comportamento deste sprite a cada frame do jogo.
            /// </summary>
            /// <returns>true se as interações físicas deverão ser processadas, false caso contrário</returns>
            protected virtual bool PreThink()
            {
                return true;
            }

            /// <summary>
            /// Evento interno que é chamado após as interações físicas deste sprite com os demais elementos do jogo forem feitas e sua posição e velocidade já tiver sido recalculadas.
            /// Sobreponha este evento em suas classes descendentes para simular um quantum de pensamento de um sprite, muito útil para a implementação de inteligência artificial.
            /// </summary>
            protected virtual void Think()
            {
            }

            /// <summary>
            /// Este evento interno é chamado após ocorrerem as interações físicas deste sprite com os demais elementos do jogo, realizada a chamada do evento Think() e feita a chamada do evento OnFrame() da classe Entity.
            /// Sobreponha este evento em suas classes descendentes se desejar realizar alguma operação final neste sprite antes do próximo tick do jogo.
            /// </summary>
            protected virtual void PostThink()
            {
            }

            /// <summary>
            /// Evento interno que é chamado antes de ocorrer a quebra desta entidade.
            /// Sobreponha este evento se quiser controlar o comportamento da quebra antes que a mesma ocorra, ou mesmo cancela-la.
            /// </summary>
            /// <returns>true se a quebra deverá ser feita, false caso contrário</returns>
            protected virtual bool OnBreak(Bomberman breaker)
            {
                return true;
            }

            /// <summary>
            /// Evento interno que é chamado após ocorrer a quebra deste sprite
            /// </summary>
            protected virtual void OnBroke(Bomberman breaker)
            {
            }

            /// <summary>
            /// Quebra o sprite!
            /// Ao quebra-lo, marca-o como quebrado, mata-o e finalmente lança o evento OnBroke()
            /// </summary>
            public void Break(Bomberman breaker)
            {
                // Verifica se ele já não está quebrado, está marcado para ser removido, se é quebrável e se a chamada ao evento OnBreak() retornou true (indicando que ele deverá ser quebrado)
                if (!broke && !markedToRemove && breakable && OnBreak(breaker))
                {
                    broke = true; // Marca-o como quebrado
                    OnBroke(breaker); // Notifica que ele foi quebrado
                    Kill(breaker); // Mate-o!
                }
            }
        }

        /// <summary>
        /// Direção que um sprite poderá assumir no jogo, sendo ela nenhuma ou uma das quatro possíveis direções: Esquerda, cima, direita e baixo.
        /// </summary>
        public enum Direction
        {
            NONE = 0, // Nenhuma
            LEFT = 1, // Esquerda
            UP = 2, // Cima
            RIGHT = 4, // Direita
            DOWN = 8 // Baixo
        }

        /// <summary>
        /// Coverte uma direção para um número inteiro
        /// </summary>
        /// <param name="direction">Direção</param>
        /// <returns>Número inteiro associado a direção dada</returns>
        public static int DirectionToInt(Direction direction)
        {
            switch (direction)
            {
                case Direction.NONE:
                    return 0;

                case Direction.LEFT:
                    return 1;

                case Direction.UP:
                    return 2;

                case Direction.RIGHT:
                    return 4;

                case Direction.DOWN:
                    return 8;
            }

            throw new InvalidOperationException("There is no integer liked to direction " + direction.ToString() + ".");
        }

        /// <summary>
        /// Converte um número inteiro para uma direção
        /// </summary>
        /// <param name="value">Número inteiro a ser convertido</param>
        /// <returns>Direção associada ao número inteiro dado</returns>
        public static Direction IntToDirection(int value)
        {
            switch (value)
            {
                case 0:
                    return Direction.NONE;

                case 1:
                    return Direction.LEFT;

                case 2:
                    return Direction.UP;

                case 4:
                    return Direction.RIGHT;

                case 8:
                    return Direction.DOWN;
            }

            throw new InvalidOperationException("There is no direction liked to value " + value + ".");
        }

        /// <summary>
        /// Calcula o logarítmo binário (ou logarítmo na base 2) de um número inteiro supostamente constituído de apenas um bit igual a 1 (ou seja, uma potência de 2)
        /// </summary>
        /// <param name="x">Operando</param>
        /// <returns>Logarítmo binário de x</returns>
        private static int Lb(int x)
        {
            int result = 0;
            int y = x;

            while (y > 1)
            {
                y >>= 1;
                result++;
            }

            return result;
        }

        /// <summary>
        /// Obtém o vetor unitário numa direção dada
        /// </summary>
        /// <param name="direction">Direção que o vetor derá ter</param>
        /// <returns>Vetor unitário na direção de direction</returns>
        public static Vector2D GetVectorDir(Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    return Vector2D.LEFT_VECTOR;

                case Direction.UP:
                    return Vector2D.UP_VECTOR;

                case Direction.RIGHT:
                    return Vector2D.RIGHT_VECTOR;

                case Direction.DOWN:
                    return Vector2D.DOWN_VECTOR;
            }

            return Vector2D.NULL_VECTOR;
        }

        /// <summary>
        /// Um sprite que se move somente nas 4 direções padrões: esquerda, cima, direta e baixo.
        /// </summary>
        public class DirectionalSprite : Sprite
        {
            protected Direction direction; // Direção para qual o sprite estará apontando, mesmo que não esteja se movendo.
            protected float speed; // Velocide escalar do sprite na direção para o qual ele aponta.

            protected Vector2D directionVector; // Vetor que aponta para a mesma direção do sprite.
            protected int currentAnimationIndex; // Animação atualmente visível.
            protected bool death; // Indica se o sprite morreu.
            private float nextDeathThink; // Quando o sprite morrer, aplica-se uma animação que terá uma duração específica. Este campo indicará o tempo do engine em que a animação terminará e o sprite será finalmente marcado para ser removido.

            /// <summary>
            /// Cria um novo sprite direcional
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do sprite</param>
            /// <param name="box">Retângulo que delimitará a área de desenho, de dano e de colisão deste sprite</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do sprite</param>
            public DirectionalSprite(FrmBomberman engine, string name, Box2D box, params ImageList[] imageLists)
            : this(engine, name, box, box, box, imageLists)
            {
            }

            /// <summary>
            /// Cria um novo sprite direcional
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do sprite</param>
            /// <param name="box">Retângulo que delimitará a área de desenho e de colisão deste sprite</param>
            /// <param name="hitBox">Retângulo que delimitará a área de dano deste sprite</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do sprite</param>
            public DirectionalSprite(FrmBomberman engine, string name, Box2D box, Box2D hitBox, params ImageList[] imageLists)
            : this(engine, name, box, hitBox, box, imageLists)
            {
            }

            /// <summary>
            /// Cria um novo sprite direcional
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do sprite</param>
            /// <param name="drawBox">Retângulo que delimitará a área de desenho deste sprite</param>
            /// <param name="hitBox">Retângulo que delimitará a área de dano deste sprite</param>
            /// <param name="collisionBox">Retângulo que delimitará a área de colisão deste sprite</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do sprite</param>
            public DirectionalSprite(FrmBomberman engine, string name, Box2D drawBox, Box2D hitBox, Box2D collisionBox, params ImageList[] imageLists)
            : base(engine, name, drawBox, hitBox, collisionBox, imageLists, false)
            {
            }

            protected override void OnCreateAnimation(int animationIndex, ref ImageList imageList, ref float fps, ref int initialFrame, ref bool startVisible, ref bool startOn, ref bool loop)
            {
                base.OnCreateAnimation(animationIndex, ref imageList, ref fps, ref initialFrame, ref startVisible, ref startOn, ref loop);
                startOn = false; // Por padrão, a animação de um sprite direcional começa parada.
                loop = animationIndex < 4; // Somente as 4 primeiras animações funcionarão em looping, que são as animações de passos para cada uma das direções.
                startVisible = animationIndex == 3; // Por padrão, um sprite direcional começa apontando para baixo, por isso somente a animação de passos para baixo ficará visível inicialmente.
            }

            public override void Spawn()
            {
                base.Spawn();

                direction = Direction.DOWN;
                speed = DEFAULT_SPEED;
                currentAnimationIndex = 3;
                death = false;
                directionVector = Vector2D.DOWN_VECTOR;
            }

            protected override void OnDeath(Bomberman killer)
            {
                death = true;
                vel = Vector2D.NULL_VECTOR;

                if (animations.Count >= 5) // Se existir pelo menos mais uma animação além das 4 animações de passos, a quinta animação será a animação de morte.
                    CurrentAnimationIndex = 4;

                nextDeathThink = engine.GetEngineTime() + DEATH_TIME; // Calcula quando o tempo da morte do sprite irá terminar. Durante este tempo a animação de morte (caso exista) estará em execução juntamente com um efeito de fade in.
                FadeIn(DEATH_TIME); // Aplica o efeito de fade in enquanto a animação de morte estiver correndo.
            }

            protected override void Think()
            {
                if (death && engine.GetEngineTime() >= nextDeathThink) // Se o sprite já morreu e se o tempo de morte já se passou,
                    Dispose(); // então disponha o sprite.
            }

            /// <summary>
            /// Como em um sprite direcional somente pode existir uma animação visível por vez, esta propriedade diz qual é a animação vísivel no momento.
            /// </summary>
            protected Animation CurrentAnimation
            {
                get
                {
                    return GetAnimation(currentAnimationIndex);
                }
            }

            /// <summary>
            /// Como em um sprite direcional somente pode existir uma animação visível por vez, esta propriedade diz qual é o índice (posição dela na lista de animações deste sprite) da animação vísivel no momento.
            /// </summary>
            protected int CurrentAnimationIndex
            {
                get
                {
                    return currentAnimationIndex;
                }
                set
                {
                    Animation animation = CurrentAnimation;
                    bool animating = animation.Animating;
                    int animationFrame = animation.CurrentFrame;
                    animation.Stop();
                    animation.Visible = false;
                    currentAnimationIndex = value;
                    animation = CurrentAnimation;
                    animation.CurrentFrame = animationFrame;
                    animation.Animating = animating;
                    animation.Visible = true;
                }
            }

            protected override void OnStartMoving()
            {
                // Quando um sprite direcional começa a se mover, sua animação de passos correspondente também deverá executar.
                Animation animation = CurrentAnimation;
                animation.Reset();
                animation.Start();
            }

            protected override void OnStopMoving()
            {
                // Quando um sprite direcional parar, sua animação de passos correspondente também deverá parar.
                Animation animation = CurrentAnimation;
                animation.Stop();
                animation.Reset();
            }

            /// <summary>
            /// Direção para o qual este sprite aponta.
            /// </summary>
            public Direction Direction
            {
                get
                {
                    return direction;
                }
                set
                {
                    if (death) // Se ele já está morto não há nada o que se fazer aqui.
                        return;

                    direction = value;
                    directionVector = GetVectorDir(direction); // Obtém o vetor direção unitário.
                    vel = directionVector * speed; // Obtém o vetor velocidade.
                    CurrentAnimationIndex = Lb(DirectionToInt(direction)); // Define qual deverá ser a animação a ser executada de acordo com a direção dada.

                    engine.Repaint(this); // Notifica o engine que este sprite deverá ser redesenhado.
                }
            }

            /// <summary>
            /// Obtém o vetor direção deste sprite.
            /// </summary>
            public Vector2D DirectionVector
            {
                get
                {
                    return directionVector;
                }
            }

            /// <summary>
            /// Velocidade escalar deste sprite.
            /// </summary>
            public float Speed
            {
                get
                {
                    return speed;
                }
                set
                {
                    speed = value;

                    for (int i = 0; i < 4; i++)
                        animations[i].FPS = DEFAULT_FPS * speed / DEFAULT_SPEED;

                    Direction = direction; // Apenas para atualizar o vetor velocidade.
                }
            }

            /// <summary>
            /// Indica se este sprite está ou não morto.
            /// </summary>
            /// <returns>true se estiver morto, false caso contrário</returns>
            public bool IsDeath()
            {
                return death;
            }

            /// <summary>
            /// Define uma direção aleatória para este sprite e inicia o movimento.
            /// </summary>
            public void RandomMove()
            {
                // Enumera todas as direções possíveis que este sprite poderá seguir.
                List<Direction> directions = new List<Direction>();

                for (int i = 0; i < 4; i++)
                {
                    // Para saber se o sprite poderá seguir em uma direção, usa-se o mesmo algoritmo de teste de colisão que é usado pelo sprite que é usado para interagir fisicamente com outros elementos do jogo.
                    Direction direction = IntToDirection(1 << i);
                    Box2D box = this.collisionBox + GetVectorDir(direction) * speed * TICK;
                    List<Entity> entities = Engine.GetOverlappedEntities(box, this, true);
                    bool cont = false;

                    foreach (Entity entity in entities)
                    {
                        if (entity is HardBlock)
                        {
                            cont = true;
                            break;
                        }

                        if (entity is Sprite)
                        {
                            Sprite sprite = (Sprite) entity;

                            if (sprite.Static || CollisionCheck(sprite))
                            {
                                cont = true;
                                break;
                            }
                        }
                    }

                    if (cont)
                        continue;

                    directions.Add(direction);
                }

                // Se existir ao menos uma direção possível,
                if (directions.Count > 0)
                {
                    int directionIndex = engine.RandomInt(directions.Count); // obtém uma direção aleatória dentro da lista de direções possívels.
                    Direction = directions[directionIndex];
                }
            }
        }

        public enum PlayerNumber
        {
            FIRST,
            SECOND
        }

        public static int PlayerNumberToInteger(PlayerNumber number)
        {
            switch (number)
            {
                case PlayerNumber.FIRST:
                    return 0;

                case PlayerNumber.SECOND:
                    return 1;
            }

            return -1;
        }

        /// <summary>
        /// Bomberman!
        /// </summary>
        public class Bomberman : DirectionalSprite
        {
            private PlayerNumber number;
            private int lives; // Quantidade de vidas.
            private float time; // Tempo (em segundos) restante que ele tem para poder completar o level atual.
            private Box2D spawnBox; // Retângulo de desenho no momento do spawn.
            private int keys; // Conjunto de teclas que estão sendo pressionadas no momento.
            private List<Bomb> armedBombs; // Lista de bombas armadas que ainda não explodiram.
            private int bombs; // Quantidade máxima de bombas que o bomberman poderá armar antes q explodam.
            private int range; // Alcance da explosão de cada bomba.
            private bool passBombs; // Indica se ele poderá atravessar bombas.
            private bool redBomb; // Indica se a bomba será vermelha. Quando bombas vermelhas explodem, sua explosão atravessa soft blocks.
            private bool remoteControl; // Indica se a bomba poderá ser detonada quando o jogador quiser e não por tempo.
            private bool kickBombs; // Indica se a bomba poderá ser chutada.
            private bool kicking; // Indica se o Bomberman está chutando no momento.
            private int score; // Quantidade de pontos que o Bomberman fez até o momento.
            private float nextTimeThink; // Tempo (em segundos) de engine no qual deverá ser notificado ao engine o próximo tick do contador de tempo do Bomberman, para que seja atualizado no top panel o tempo do jogo.

            /// <summary>
            /// Cria um novo Bomberman
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do Bomberman</param>
            /// <param name="box">Retângulo de desenho do Bomberman</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do Bomberman</param>
            public Bomberman(FrmBomberman engine, string name, PlayerNumber number, Box2D box, ImageList[] imageLists)
            // Dado o retângulo de desenho do Bomberman, o retângulo de colisão será a metade deste enquanto o de dano será um pouco menor ainda.
            // A posição do retângulo de colisão será aquela que ocupa a metade inferior do retângulo de desenho enquanto o retângulo de dano terá o mesmo centro que o retângulo de colisão.
            : base(engine, name, box, new Box2D(new Vector2D(box.Origin.X, box.Origin.Y + box.Height / 2), Vector2D.NULL_VECTOR, new Vector2D(box.Width, box.Height / 2)) - 16, new Box2D(new Vector2D(box.Origin.X, box.Origin.Y + box.Height / 2), Vector2D.NULL_VECTOR, new Vector2D(box.Width, box.Height / 2)), imageLists)
            {
                this.number = number;
            }

            protected override void OnHealthChanged(float health)
            {
                engine.RepaintHeart(); // Notifica o engine que o HP do Bomberman foi alterado para que seja redesenhado no top panel.
            }

            /// <summary>
            /// Quantidade de vidas que o Bomberman possui.
            /// </summary>
            public int Lives
            {
                get
                {
                    return lives;
                }
                set
                {
                    lives = value;
                    engine.RepaintLives();
                }
            }

            /// <summary>
            /// Tempo restante (em segundos) que o Bomberman possui para terminar o level atual. O tempo é reiniciado a cada respawn.
            /// </summary>
            public float Time
            {
                get
                {
                    return time;
                }
                set
                {
                    time = value;
                    Engine.RepaintTime();
                }
            }

            /// <summary>
            /// Alcance da explosão das bombas.
            /// </summary>
            public int Range
            {
                get
                {
                    return range;
                }
                set
                {
                    range = value;
                    engine.RepaintFire();
                }
            }

            /// <summary>
            /// Indica se as próximas bombas a serem plantadas serão vermelhas. Quando uma bomba vermelha explode, sua explosão atravessa soft blocks.
            /// </summary>
            public bool RedBomb
            {
                get
                {
                    return redBomb;
                }
                set
                {
                    redBomb = value;
                }
            }

            /// <summary>
            /// Indica se o Bomberman poderá atravessar bombas.
            /// </summary>
            public bool PassBombs
            {
                get
                {
                    return passBombs;
                }
                set
                {
                    passBombs = value;
                }
            }

            /// <summary>
            /// Quantidade de bombas que o Bomberman armou antes que elas tenham explodido.
            /// </summary>
            public int ArmedBombs
            {
                get
                {
                    return armedBombs.Count;
                }
            }

            /// <summary>
            /// Quantidade máxima de bombas que o Bomberman poderá armar antes que explodam.
            /// </summary>
            public int Bombs
            {
                get
                {
                    return bombs;
                }
                set
                {
                    bombs = value;
                    engine.RepaintBombs();
                }
            }

            /// <summary>
            /// Indica se as próximas bombas que o Bomberman armar poderão ser detonadas quando ele quiser e não somente por tempo.
            /// </summary>
            public bool RemoteControl
            {
                get
                {
                    return remoteControl;
                }
                set
                {
                    remoteControl = value;
                }
            }

            /// <summary>
            /// Indica se o Bomberman poderá chutar bombas.
            /// </summary>
            public bool KickBombs
            {
                get
                {
                    return kickBombs;
                }
                set
                {
                    kickBombs = value;
                }
            }

            /// <summary>
            /// Retângulo de desenho do Bomberman durante seu spawn.
            /// </summary>
            public Box2D SpawnBox
            {
                get
                {
                    return spawnBox;
                }
            }

            /// <summary>
            /// Quantidade de pontos que o Bomberman fez desde o início do jogo.
            /// </summary>
            public int Score
            {
                get
                {
                    return score;
                }
                set
                {
                    score = value;
                    engine.RepaintScore();
                }
            }

            public override void Spawn()
            {
                base.Spawn();

                lives = INITIAL_LIVES;
                time = INITIAL_TIME;
                bombs = 1;
                range = 1;
                spawnBox = drawBox;

                keys = 0;
                armedBombs = new List<Bomb>();
                passBombs = false;
                redBomb = false;
                remoteControl = false;
                kickBombs = false;
                kicking = false;

                nextTimeThink = engine.GetEngineTime() + 1; // Atualiza o tempo em que ocorrerá o próximo tick do relógio que marca o tempo restante de jogo que o Bomberman devera ter. Cada tick é de um segundo.

                invincibilityTime = BOMBERMAN_INVINCIBILITY_TIME;
                MakeInvincible(); // Toda vez que o Bomberman spawna, ele fica invencível por um determinado tempo.
            }

            protected override void OnDeath(Bomberman killer)
            {
                // Toda vez que o bomberman morre,
                Lives--; // decrementa sua quantidade de vidas.
                kicking = false; // Se ele estava chutando, não estará mais.

                engine.PlaySound("TIME_UP"); // Toca o som de morte do Bomberman.

                base.OnDeath(killer); // Chama o método OnDeath() da classe base.

                if (lives > 0) // Se ele ainda possuir vidas,
                    engine.ScheduleRespawn(this); // respawna o Bomberman.
                else
                    engine.OnGameOver(); // Senão, Game Over!
            }

            protected override void Think()
            {
                // Verifica se é hora de mais um tick do relógio do top panel.
                if (TIME_CONTROL)
                {
                    time -= TICK;

                    if (engine.GetEngineTime() >= nextTimeThink)
                    {
                        nextTimeThink = engine.GetEngineTime() + 1;
                        engine.RepaintTime();
                    }

                    if (time <= 0) // Se acabou o tempo do Bomberman,
                    {
                        time = 0;
                        Break(null); // bye bye!
                    }
                }

                base.Think();
            }

            protected override bool ShouldCollide(Sprite sprite)
            {
                return !passBombs && sprite is Bomb; // O Bomberman só colidirá com outros sprites se forem bombas e se ele não tiver obtido o sprite de atravessar bombas ainda.
            }

            protected override void OnStartTouch(Sprite sprite)
            {
                // Quando o Bomberman tocar em algum sprite.
                if (sprite is PowerUp) // Se ele for um powerup,
                {
                    sprite.Break(null); // remova o powerup.
                    Score += POWERUP_POINTS; // Atualiza o score do Bomberman.

                    // Verifica qual tipo de powerup era.
                    switch (((PowerUp) sprite).Type)
                    {
                        case PowerUpType.BOMB_PASS: // Se for o atravessador de bombas,
                            passBombs = true; // agora eu posso atravessar bombas.
                            break;

                        case PowerUpType.BOMB_UP: // Se for o incrementador de bombas
                            if (bombs < MAX_BOMBS) // e eu já não estiver com o número máximo de bombas,
                                Bombs++; // incremente meu número de bombas.

                            break;

                        case PowerUpType.CLOCK: // Se for o relógio,
                            time += INITIAL_TIME; // incrementa meu tempo.

                            if (time > MAX_TIME) // Se meu tempo for maior que o máximo tempo permitido,
                                time = MAX_TIME; // trunca para o máximo tempo permitido.

                            break;

                        case PowerUpType.FIRE_UP: // Se for o incrementador de range
                            if (range < MAX_RANGE) // e eu ainda não possuir a range máxima ainda,
                                Range++; // incremente minha range.

                            break;

                        case PowerUpType.HEART: // Se for o coração,
                            Health += DEFAULT_HEALTH; // aumente meu HP.
                            break;

                        case PowerUpType.KICK: // Se for o chutador de bombaas,
                            kickBombs = true; // agora posso chutar bombas.
                            break;

                        case PowerUpType.LIFE: // Se for uma vida extra
                            if (lives < MAX_LIVES) // e eu ainda não possuir o número máximo de vidas,
                                Lives++; // incremente minhas vidas.

                            engine.PlaySound("P1UP"); // Toque o som de vida extra obtida.
                            return; // Caia fora!

                        case PowerUpType.RED_BOMB: // Se for a bomba vermelha,
                            redBomb = true; // agora posso plantar bombas vermelhas.
                            break;

                        case PowerUpType.REMOTE_CONTROL: // Se for a bomba de controle remoto,
                            remoteControl = true; // agora posso plantar bombas que poderão ser detonadas quando eu quiser.
                            break;

                        case PowerUpType.ROLLER: // Se for o patins
                            if (Speed == DEFAULT_SPEED) // e minha velocidade ainda for a normal,
                                Speed = 2 * DEFAULT_SPEED; // dobre minha velocidade.

                            break;

                        //case PowerUpType.SKULL:
                        //    break;
                        case PowerUpType.SOFT_BLOCK_PASS: // Se for o atravessador de soft blocks,
                            PassStatics = true; // agora posso atravessar soft blocks.
                            break;

                        case PowerUpType.VEST: // Se for o colete,
                            MakeInvincible(VEST_TIME); // me torne invencível por um determinado tempo.
                            break;
                    }

                    engine.PlaySound("ITEM_GET"); // Toca o som de powerup adquirido.
                }
                else if (sprite is Portal && engine.enemyCount == 0) // Senão, se ele for um portal e o level atual já não tiver mais inimigos vivos,
                {
                    engine.PlaySound("GOAL"); // toca o som de level terminado.

                    Score += (int) time; // Incrementa meus pontos com a quantidade de tempo restante (em segundos) que marcava meu relógio.
                    engine.NextLevel(); // Notifica o engine para que seja carregado o próximo level.
                }
            }

            /// <summary>
            /// Obtém a primeira direção possível para um dado conjunto de teclas pressionadas.
            /// </summary>
            /// <param name="bits">Conjunto de bits que indicam quais teclas estão sendo pressionadas.</param>
            /// <returns></returns>
            private static Direction FirstDirection(int bits)
            {
                for (int i = 0; i < 4; i++)
                {
                    int mask = 1 << i;

                    if ((bits & mask) != 0)
                        return IntToDirection(mask);
                }

                return Direction.NONE;
            }

            /// <summary>
            /// Atualiza o conjunto de teclas que estão sendo pressionadas.
            /// </summary>
            /// <param name="value">Conjunto de teclas pressionadas.</param>
            private void UpdateKeys(int value)
            {
                if (death)
                    return;

                keys = value;

                if (!kicking)
                {
                    Direction direction = FirstDirection(value);

                    if (direction != Direction.NONE)
                        Direction = direction;
                    else
                    {
                        Direction = this.direction;
                        vel = Vector2D.NULL_VECTOR;
                    }
                }
            }

            /// <summary>
            /// Conjunto de teclas que estão sendo pressionadas pelo jogador que controla o Bomberman.
            /// </summary>
            public int Keys
            {
                get
                {
                    return keys;
                }
                set
                {
                    if (keys != value)
                        UpdateKeys(value);
                }
            }

            /// <summary>
            /// Planta uma bomba.
            /// </summary>
            public void DropBomb()
            {
                if (death) // Se ele está morto, não há nada o que se fazer aqui.
                    return;

                if (armedBombs.Count < bombs) // Se ele ainda não plantou o número máximo de bombas simultâneas.
                {
                    Bomb bomb = engine.SpawnBomb(this); // Peça ao engine para ser plantada uma nova bomba na posição em que o Bomberman está.

                    if (bomb != null) // Se o engine aceitou o pedido (se a posição em que o Bomberman estava era livre, por exemplo),
                    {
                        armedBombs.Add(bomb); // Adiciona a nova bomba a lista de bombas armadas.
                        engine.PlaySound("BOM_SET"); // Toca o som de bomba armada.
                    }
                }
            }

            /// <summary>
            /// Detona a útlima bomba plantada, caso eu possa detonar bombas por controle remoto.
            /// </summary>
            public void Detonate()
            {
                if (death || !remoteControl) // Se eu estiver morto ou se eu não possuir a habilidade de detonar bombas por controle remoto então não há nada oque ser fazer aqui.
                    return;

                // Verifique a lista de bombas armadas desde o início e veja qual é a primeira bomba plantada desde que eu adquiri a habilidade de detonar bombas por controle remoto.
                for (int i = 0; i < armedBombs.Count; i++)
                {
                    Bomb bomb = armedBombs[i];

                    if (!bomb.Timed) // Se for encontrada tal bomba
                    {
                        bomb.Explode(); // então exploda-a
                        return; // e caia fora!
                    }
                }
            }

            /// <summary>
            /// Chute
            /// </summary>
            public void Kick()
            {
                // Se eu estiver porto, se não puder chutar bombas ou se eu já estiver chutando, então não há nada o que se fazer aqui.
                if (death || !kickBombs || kicking)
                    return;

                kicking = true; // Agora estou chutando.
                vel = Vector2D.NULL_VECTOR; // Enquanto eu estiver chutando, não poderem me mover.
                                            // Para o Bomberman, além das 4 primeiras animações que correspondem as animações de movimento
                                            // e a quinta animação que corresponde a animação de morte,
                                            // temos mais 4 animações que correspondem as animações de chute, uma pra cada direção.
                CurrentAnimationIndex = currentAnimationIndex + 5; // Atualiza a animação atual para a animação de chute correspondente a direção no qual estou apontando.
                CurrentAnimation.StartFromBegin(); // Execute-a.

                Box2D box = collisionBox + directionVector; // Obtenha o retângulo que cobre a área de chute.
                List<Entity> overlappeds = engine.GetOverlappedEntities(box, this, true, false); // Obtenha as entidades que interseptam este retângulo.

                foreach (Entity entity in overlappeds) // Verifique quais delas é uma bomba.
                {
                    if (entity is Bomb) // Se ela for uma,
                        ((Bomb) entity).Velocity = BOMB_SPEED * directionVector; // coloque-a para mover na velocidade padrão da bomba e na direção para o qual eu estou apontando.
                }
            }

            /// <summary>
            /// Evento que ocorrerá sempre que uma de minhas bombas explodir.
            /// </summary>
            /// <param name="bomb">Bomba que explodiu</param>
            public void OnBombExploded(Bomb bomb)
            {
                if (armedBombs.Contains(bomb)) // Verifique se é mesmo uma de minhas bombas que eu havia armado antes.
                    armedBombs.Remove(bomb); // Se for, remova-a de minha lista de bombas armadas.
            }

            internal override void OnAnimationEnd(Animation animation)
            {
                // Sempre que uma de minhas animações tiver sua execução chegada ao fim, ou seja, tiver atingido o último frame.
                // Verifique se eu estava chutando e se esta animação é a uma das animações de chute. Caso seja, então...
                if (kicking && animation.Index >= 5)
                {
                    kicking = false; // não estou mais chutando.
                    CurrentAnimationIndex = currentAnimationIndex - 5; // Atualiza a animação atual para a animação de movimento que eu possuia antes de chutar.
                    CurrentAnimation.Stop(); // Mas eu não estou me movimentando, então pare a animação caso ela esteja em execução
                    CurrentAnimation.Reset(); // e atualize o quadro atual para o quadro inicial.
                    UpdateKeys(keys); // Caso eu estava pressionado uma ou mais teclas antes, atualize-as para que tudo continue funcionando bem.
                }
            }

            protected override void OnStartMoving()
            {
                if (!kicking) // Se eu estiver chutando, não notifique a classe base de que eu comecei a me mover para que não ocorra a troca de animação.
                    base.OnStartMoving();
            }

            protected override void OnStopMoving()
            {
                if (!kicking) // Se eu estiver chutando, não notifique a classe base que eu parei de me mover para que não ocorra a troca de animação.
                    base.OnStopMoving();
            }
        }

        /// <summary>
        /// Inimigo que pode ferir ou matar o Bomberman.
        /// </summary>
        public class Enemy : DirectionalSprite
        {
            private int points; // Pontos que esse inimigo dará ao Bomberman se for morto por ele.

            /// <summary>
            /// Cria um novo inimigo
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="name">Nome do inimigo</param>
            /// <param name="points">Pontos que o inimigo dará ao Bomberman quando for morto por ele</param>
            /// <param name="box">Retângulo de desenho, de dano e de colisão do inimigo</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do inimigo</param>
            public Enemy(FrmBomberman engine, string name, int points, Box2D box, ImageList[] imageLists)
            : base(engine, name, box, box, imageLists)
            {
                this.points = points;
            }

            /// <summary>
            /// Pontos que o inimigo dará ao Bomberman quanfo for morto por ele.
            /// </summary>
            public int Points
            {
                get
                {
                    return points;
                }
            }

            public override void Spawn()
            {
                base.Spawn();

                Direction = IntToDirection(1 << engine.RandomInt(4));
                engine.enemyCount++; // Quando um inimigo spawnar, notifique ao engine que o número de inimigos aumentou.
            }

            protected override void OnDeath(Bomberman killer)
            {
                engine.enemyCount--; // E quando um inimigo morrer, notifique ao engine que o número de inimigos diminuiu

                if (killer != null)
                    killer.Score += points; // e incrementa os pontos do jogador que eliminou este inimigo.

                base.OnDeath(killer);
            }

            protected override void OnStartTouch(Sprite sprite)
            {
                CheckTouching(sprite);
            }

            protected override void OnTouching(Sprite sprite)
            {
                CheckTouching(sprite);
            }

            private void CheckTouching(Sprite sprite)
            {
                if (sprite is Bomberman) // Se eu comecei a tocar ou e estiver tocando o Bomberman,
                    Hurt(sprite, null, DEFAULT_HEALTH); // Cause dano a ele, caso eu esteja tocando sua hitbox (o que será verificado dentro do método Hurt()).
            }
        }

        /// <summary>
        /// Inimigo que anda sempre que tiver uma direção a seguir.
        /// Ele somente irá mudar de direção quando colidir com um bloco, uma bomba ou com outro creep.
        /// A direção que o creep irá tomar a cada mudança de direção será sempre aleatória.
        /// O creep possui a quantidade de HP normal (a mesma que o Bomberman tem inicialmente), logo se ele for atingido por uma explosão morrerá na hora.
        /// </summary>
        public class Creep : Enemy
        {
            public Creep(FrmBomberman engine, string name, int points, Box2D box, ImageList[] imageLists)
            : base(engine, name, points, box, imageLists)
            {
            }

            protected override bool ShouldCollide(Sprite sprite)
            {
                return sprite is Creep || sprite is Bomb; // Como dito antes, só colido com um sprite se ele for uma bomba ou se for outro creep.
            }

            protected override void Think()
            {
                base.Think();

                if (!death && !moving) // E como dito antes, só mudo de direção se eu colidir com algo (e se eu não estiver morto, é claro). Sempre que um sprite colide com algo ele para de se mover.
                    RandomMove(); // A direção que o creep irá tomar será sempre aleatória, como foi dito antes.
            }

        }

        /// <summary>
        /// Inimigo um pouco mais esperto que persegue o Bomberman e possui o dobro de HP que o creep,
        /// portanto é necessário atingi-lo com pelo menos duas explosões para que ele morra.
        /// </summary>
        public class Cactus : Enemy
        {
            public Cactus(FrmBomberman engine, string name, int points, Box2D box, ImageList[] imageLists)
            : base(engine, name, points, box, imageLists)
            {
            }

            public override void Spawn()
            {
                base.Spawn();

                health = 2 * DEFAULT_HEALTH;
            }

            protected override bool ShouldCollide(Sprite sprite)
            {
                return sprite is Bomb && sprite.Velocity == Vector2D.NULL_VECTOR; // Só irei colidir com bombas se ela não estiver se movendo.
            }

            protected override void Think()
            {
                base.Think();

                if (death) // Se eu já estiver morto, não há nada o que se fazer aqui.
                    return;

                // Verifique se eu estou na posição exata de uma célula do jogo.
                Vector2D mins = collisionBox.Origin + collisionBox.Mins;
                int x = (int) mins.X;
                int y = (int) mins.Y;
                Vector2D pos = new Vector2D(x, y);
                TeleportTo(pos);

                // Se eu não estiver
                if (x % CELL_SIZE != 0 || y % CELL_SIZE != 0)
                {
                    if (!moving) // e eu não estiver me movendo,
                        RandomMove(); // tome um rumo qualquer

                    return; // e caia fora daqui.
                }

                // Se o Bomberman estiver morto
                if (engine.IsAllPlayersDied())
                {
                    RandomMove(); // tome um rumo qualuer e também caia fora daqui.
                    return;
                }

                // Aqui está a utilidade do grafo bidimensional!
                // Com o uso dele posso computar o menor caminho entre mim e o Bomberman caso exista algum caminho até ele.
                Graph2DCoords src = GetGraphCoords(collisionBox);

                if (src == null)
                {
                    RandomMove();
                    return;
                }

                Graph2DCoords dst = GetGraphCoords(engine.player[0].CollisionBox);

                if (dst == null)
                {
                    if (engine.player[1] == null)
                    {
                        RandomMove();
                        return;
                    }

                    dst = GetGraphCoords(engine.player[1].CollisionBox);
                    if (dst == null)
                    {
                        RandomMove();
                        return;
                    }
                }

                List<Graph2DCoords> routeCoords = new List<Graph2DCoords>();
                engine.GenerateGraphNodeValues(dst); // Notifique ao engine que deverá ser atualizado os valores dos nós dos grafos com as distâncias relativas até o Bomberman.

                if (DEBUG_ROUTE)
                {
                    List<Direction> route = new List<Direction>();

                    if (engine.graph.GetMinimalRoute(src, routeCoords))
                    {
                        if (route.Count > 1)
                        {
                            Graph2DCoords src1 = routeCoords[0];

                            for (int i = 1; i < route.Count; i++)
                            {
                                dst = routeCoords[i];
                                int dx = dst.Col - src1.Col;
                                int dy = dst.Row - src1.Row;

                                if (dx == 1 && dy == 0)
                                    route.Add(Direction.RIGHT);
                                else if (dx == -1 && dy == 0)
                                    route.Add(Direction.LEFT);
                                else if (dx == 0 && dy == 1)
                                    route.Add(Direction.DOWN);
                                else if (dx == 0 && dy == -1)
                                    route.Add(Direction.UP);

                                src1 = dst;
                            }
                        }

                        engine.SetDebugPath(collisionBox, route);
                    }
                }

                Graph2D.Node nextNode = engine.graph.GetNextNode(src); // Obtenha o próximo nó do grafo que eu deverei seguir para chegar ao Bomberman.

                if (nextNode == null) // Se esse nó não existir
                {
                    RandomMove(); // tome um rumo qualquer e caia fora.
                    return;
                }

                // Como deu tudo certo até aqui, por fim atualize a minha direção.
                int dx1 = nextNode.Col - src.Col;
                int dy1 = nextNode.Row - src.Row;

                if (dx1 == 1 && dy1 == 0)
                    Direction = Direction.RIGHT;
                else if (dx1 == -1 && dy1 == 0)
                    Direction = Direction.LEFT;
                else if (dx1 == 0 && dy1 == 1)
                    Direction = Direction.DOWN;
                else if (dx1 == 0 && dy1 == -1)
                    Direction = Direction.UP;
            }

        }

        /// <summary>
        /// Uma bomba plantada
        /// </summary>
        public class Bomb : Sprite
        {
            private Bomberman bomberman; // Bomberman que a plantou
            private bool red; // Indica se a bomba é vermelha ou não
            private int range; // Range da explosão da bomba
            private float elapsed; // Tempo (em segundos) decorrido desde que a bomba foi plantada
            private bool exploding; // Indica se a bomba está explodindo
            private bool exploded; // Indica se a bomba já explodiu
            private bool timed; // Indica se a bomba irá explodir por tempo (true) ou por controle remoto (false)

            public Bomb(FrmBomberman engine, Bomberman bomberman, Box2D box, params ImageList[] imageLists)
            : base(engine, "Bomb", box - 8, box, imageLists, false)
            {
                this.bomberman = bomberman;
            }

            /// <summary>
            /// Bomberman que plantou a bomba
            /// </summary>
            public Bomberman Bomberman
            {
                get
                {
                    return bomberman;
                }
            }

            /// <summary>
            /// Alcance da explosão da bomba
            /// </summary>
            public float Range
            {
                get
                {
                    return range;
                }
            }

            /// <summary>
            /// Indica se a bomba é vermelha
            /// </summary>
            public bool Red
            {
                get
                {
                    return red;
                }
            }

            /// <summary>
            /// Indica se a bomba explodirá somente por tempo
            /// </summary>
            public bool Timed
            {
                get
                {
                    return timed;
                }
            }

            protected override bool ShouldCollide(Sprite sprite)
            {
                return sprite is Bomb; // Bombas colidem com outras bombas.
            }

            public override void Spawn()
            {
                base.Spawn();

                range = bomberman.Range;
                red = bomberman.RedBomb;
                timed = !bomberman.RemoteControl;
                elapsed = 0;
                exploding = false;
                exploded = false;
            }

            protected override bool OnBreak(Bomberman breaker)
            {
                if (exploded)
                    return true;

                if (exploding)
                    return false;

                Explode(); // Toda vez que uma bomba quebra, ela explode.
                return false;
            }

            protected override void OnStartMoving()
            {
                // Toda vez que uma bomba começa a se mover, deve ser atualizado o grafo e tocar o som de início de movimento da bomba.
                Graph2DCoords node = GetGraphCoords(collisionBox);

                if (node != null)
                {
                    engine.graph.Insert(node);
                    Graph2DCoords dst = GetGraphCoords(bomberman.CollisionBox);

                    if (dst != null)
                        engine.GenerateGraphNodeValues(dst);

                    engine.PlaySound("BOM_KICK");

                    if (DEBUG_GRAPH)
                        engine.Invalidate();
                }
            }

            protected override void OnStopMoving()
            {
                // Toda vez que uma bomba para de se mover, deve ser atualizado o grafo e tocar o som de quando uma bomba para de se mover.
                Graph2DCoords node = GetGraphCoords(collisionBox);

                if (node != null)
                {
                    engine.graph.Delete(node);
                    Graph2DCoords dst = GetGraphCoords(bomberman.CollisionBox);

                    if (dst != null)
                        engine.GenerateGraphNodeValues(dst);

                    engine.PlaySound("BOM_BOUND");

                    if (DEBUG_GRAPH)
                        engine.Invalidate();
                }
            }

            /// <summary>
            /// Explode a bomba
            /// </summary>
            public void Explode()
            {
                // Se a bomba já quebrou, se já foi marcada pra remover, se já está explodindo ou se já explodiu, então não há nada o que se fazer aqui.
                if (broke || markedToRemove || exploding || exploded)
                    return;

                exploding = true; // Estou explodindo.
            }

            /// <summary>
            /// Toca o som de explosão da bomba
            /// </summary>
            private void PlayBombSound()
            {
                if (range < 4) // Se a range for menor que 4
                    engine.PlaySound("BOM_11_S"); // toca o som de explosão fraco.
                else if (range < 8) // Se a range estiver entre 4 (inclusive) e 8 (exclusive)
                    engine.PlaySound("BOM_11_M"); // toca o som de explosão médio.
                else // Se for maior ou igual a 8
                    engine.PlaySound("BOM_11_L"); // toca o som de explosão forte.
            }

            protected override void Think()
            {
                base.Think();

                // Se eu não estiver explodindo e se eu explodo somente com tempo
                if (!exploding && timed)
                {
                    elapsed += TICK; // atualize meu tempo

                    if (elapsed >= BOMB_TIME) // e verifique se ele já não se esgotou. Caso tenha se esgotado,
                        Explode(); // me exploda!
                }
                else if (exploding) // Senão, se eu já estiver explodindo, somente poderei emitir as chamas quando eu estiver exatamente na mesma posição de uma célula.
                {
                    Vector2D mins = collisionBox.Origin + collisionBox.Mins;
                    int x = (int) mins.X;
                    int y = (int) mins.Y;

                    if (x % CELL_SIZE != 0 || y % CELL_SIZE != 0) // Se eu não estiver exatamente dentro de uma única célula...
                    {
                        // ... então calcule as coordenadas da célula mais próxima
                        int x0 = (x / CELL_SIZE) * CELL_SIZE;
                        x = x - x0 <= x0 + CELL_SIZE - x ? x0 : x0 + CELL_SIZE;

                        int y0 = (y / CELL_SIZE) * CELL_SIZE;
                        y = y - y0 <= y0 + CELL_SIZE - y ? y0 : y0 + CELL_SIZE;
                    }

                    TeleportTo(new Vector2D(x, y)); // posiciona na célula mais próxima
                    vel = Vector2D.NULL_VECTOR; // Pare meu movimento.
                    exploding = false; // Não estou mais explodindo
                    exploded = true; // pos eu já explodi!
                    bomberman.OnBombExploded(this); // Notifica ao Bomberman que eu explodi.
                    Break(null); // Me quebre!
                    engine.SpawnFlames(this, range); // Spawne as minhas chamas.

                    // Atualize o grafo.
                    Graph2DCoords node = GetGraphCoords(collisionBox);

                    if (node != null)
                    {
                        engine.graph.Insert(node);
                        Graph2DCoords dst = GetGraphCoords(bomberman.CollisionBox);

                        if (dst != null)
                            engine.GenerateGraphNodeValues(dst);

                        if (DEBUG_GRAPH)
                            engine.Invalidate();
                    }

                    PlayBombSound(); // Toca o som de explosão.
                }
            }
        }

        /// <summary>
        /// Chamas de uma explosão
        /// </summary>
        public class Flame : Sprite
        {
            private Bomberman player;

            public Bomberman Player
            {
                get
                {
                    return player;
                }
            }

            public Flame(FrmBomberman engine, Bomberman player, Box2D box, params ImageList[] imageLists)
            : base(engine, "Flame", box, box, imageLists, false)
            {
                this.player = player;
            }

            public override void Spawn()
            {
                base.Spawn();

                FadeIn(FLAME_TIMELIFE); // Quando uma chama acaba de surgir, ela deve sumir lentamente com o tempo.
            }

            protected override void OnStartTouch(Sprite sprite)
            {
                CheckTouching(sprite);
            }

            protected override void OnTouching(Sprite sprite)
            {
                CheckTouching(sprite);
            }

            private void CheckTouching(Sprite sprite)
            {
                if (Opacity >= MININAL_OPACITY_TO_HURT) // Se uma chama toca alguém e sua opacidade for pelo menos um determinado valor
                    Hurt(sprite, player, DEFAULT_HEALTH); // cause dano neste alguém desde que toque sua hit box (o que será verificado dentro da chamada Hurt()).
            }

            protected override void OnFadeInComplete()
            {
                Kill(null); // Quando o efeito de fade in terminar, me mate!
            }
        }

        /// <summary>
        /// Tipo de powerups
        /// </summary>
        public enum PowerUpType
        {
            BOMB_PASS = 0, // Atravessador de bombas
            BOMB_UP = 1, // Incrementador de bombas
            CLOCK = 2, // Relógio (aumenta o tempo do Bomberman)
            FIRE_UP = 3, // Incrementador de range
            HEART = 4, // Coração (aumenta o HP do Bomberman)
            KICK = 5, // Chutador de bombas
            LIFE = 6, // Vida extra
            RED_BOMB = 7, // Bomba vermelha (quando explodem, suas chamas atravessam soft blocks)
            REMOTE_CONTROL = 8, // Controle remoto (permite ao Bomberman detonar bombas quando quiser)
            ROLLER = 9, // Patins (aumenta a velocidade do Bomberman)
                        //SKULL = 10,
            SOFT_BLOCK_PASS = 10, // Atravessador de soft blocks
            VEST = 11 // Colete (torna o Bomberman invencível por um determinado tempo)
        }

        /// <summary>
        /// Powerup
        /// </summary>
        public class PowerUp : Sprite
        {
            private PowerUpType type; // Tipo do powerup

            public PowerUp(FrmBomberman engine, PowerUpType type, Box2D box, params ImageList[] imageLists)
            : base(engine, type.ToString(), box - POWERUP_DRAWBOX_BORDER_SIZE, box, imageLists, false)
            {
                this.type = type;
            }

            protected override void OnCreateAnimation(int animationIndex, ref ImageList imageList, ref float fps, ref int initialFrame, ref bool startVisible, ref bool startOn, ref bool loop)
            {
                base.OnCreateAnimation(animationIndex, ref imageList, ref fps, ref initialFrame, ref startVisible, ref startOn, ref loop);
                startOn = false; // Powerups não possuem animação
                initialFrame = (int) type; // mas possuem quadros de animação, e o o quadro a ser exibido será o quadro correspondente ao tipo do powerup.
            }

            /// <summary>
            /// Tipo do powerup
            /// </summary>
            public PowerUpType Type
            {
                get
                {
                    return type;
                }
                set
                {
                    type = value;
                    GetAnimation(0).CurrentFrame = (int) type;
                    engine.Repaint(this);
                }
            }
        }

        /// <summary>
        /// Blocos que podem ser quebrados por explosões
        /// </summary>
        public class SoftBlock : Sprite
        {
            private Sprite prize; // Sprite que irá surgir no lugar do bloco quando ele se quebrar, geralmente um powerup mas poderá ser também um portal.
            protected bool breaking; // Indica se o bloco está se quebrando.
            protected Bomberman breaker;
            private float nextBreakThink; // Tempo (sem segundos) de quando a quebra do bloco terminará.

            /// <summary>
            /// Cria um novo bloco quebrável
            /// </summary>
            /// <param name="engine">Engine</param>
            /// <param name="box">Retângulo de desenho, dano e colisão</param>
            /// <param name="prize">Sprite que irá surgir no lugar do bloco quando ele se quebrar, geralmente um powerup mas poderá ser também um portal</param>
            /// <param name="imageLists">Array de lista de imagens que serão usadas na animação do soft block.</param>
            public SoftBlock(FrmBomberman engine, Box2D box, Sprite prize, params ImageList[] imageLists)
            : base(engine, "SoftBlock", box, box, imageLists, false)
            {
                this.prize = prize;
            }

            /// <summary>
            /// Sprite que irá surgir no lugar do bloco quando ele se quebrar, geralmente um powerup mas poderá ser também um portal
            /// </summary>
            public Sprite Prize
            {
                get
                {
                    return prize;
                }
            }

            public override void Spawn()
            {
                base.Spawn();

                isStatic = true; // Todo soft block é um sprite estático, ou seja, as interações físicas de outro sprite com ele é tratada como se fosse com um hard block.
                breaking = false;
            }

            protected override void OnCreateAnimation(int animationIndex, ref ImageList imageList, ref float fps, ref int initialFrame, ref bool startVisible, ref bool startOn, ref bool loop)
            {
                base.OnCreateAnimation(animationIndex, ref imageList, ref fps, ref initialFrame, ref startVisible, ref startOn, ref loop);
                startOn = false; // Soft blocks não possuem animação quando acabam de ser criados
                loop = false; // mas terão animação quando começarem a quebrar, mas esta animação não será em looping.
                fps = (imageList.Images.Count - 1) / SOFT_BLOCK_BREAK_TIME; // Como não existe loop, o número de quadros por segundo deverá tal que a animação dure exatamente o tempo de quebra do soft block.
            }

            protected override void Think()
            {
                if (breaking && engine.GetEngineTime() >= nextBreakThink) // Se eu estava quebrando e já se passou o tempo de quebra
                    Break(breaker); // Me quebre denovo (jajá vão entender o porque)!
            }

            protected override bool OnBreak(Bomberman breaker)
            {
                if (!breaking) // Na primeira quebra, o sprite exibe a animação de quebra.
                {
                    breaking = true; // Estou quebrando.
                    this.breaker = breaker;
                    nextBreakThink = engine.GetEngineTime() + SOFT_BLOCK_BREAK_TIME; // Calcula o tempo em que a quebra irá terminar.
                    animations[0].Start(); // Inicia a animação de quebra.

                    return false; // Não devo ser removido ainda.
                }

                return engine.GetEngineTime() >= nextBreakThink; // Senão, caso já esteja quebrando, só serei removido realmente se o meu tempo de quebra já chegou ao fim.
            }

            protected override void OnBroke(Bomberman breaker)
            {
                // E quando eu finalmente quebrar
                if (breaker != null)
                    breaker.Score += SOFT_BLOCK_POINTS; // atualiza os pontos do Bomberman.

                if (prize != null) // Se havia um prêmio
                    prize.Spawn(); // spawne este prêmio

                // E por fim, atualize o grafo.
                Graph2DCoords node = GetGraphCoords(collisionBox);

                if (node != null)
                {
                    engine.graph.Insert(node);

                    if (DEBUG_GRAPH)
                        engine.Invalidate();
                }
            }
        }

        /// <summary>
        /// Portal de saída do level para o Bomberman.
        /// Quando o Bomberman conseguir matar todos os inimigos do level e achar onde está o portal ele finalmente poderá ir para o próximo level.
        /// </summary>
        public class Portal : Sprite
        {
            public Portal(FrmBomberman engine, Box2D box, params ImageList[] imageLists)
            : base(engine, "Portal", box, box, imageLists, false)
            {
            }

            public override void Spawn()
            {
                base.Spawn();

                breakable = false; // Portais não podem ser quebrados.
            }
        }

        /// <summary>
        /// Classe auxiliar usada para armazenar informações de respawn
        /// </summary>
        public class RespawnEntry
        {
            private Bomberman bomberman; // Bomberman que será respawnado
            private float time; // Tempo que deverá se esperar para que o respawn ocorra

            /// <summary>
            /// Cria uma nova entrada de respawn
            /// </summary>
            /// <param name="bomberman">Bomberman que será respawnado</param>
            /// <param name="time">Tempo que deverá se esperar para que o respawn ocorra</param>
            public RespawnEntry(Bomberman bomberman, float time)
            {
                this.bomberman = bomberman;
                this.time = time;
            }

            /// <summary>
            /// Bomberman que será respawnado
            /// </summary>
            public Bomberman Bomberman
            {
                get
                {
                    return bomberman;
                }
            }

            /// <summary>
            /// Tempo que deverá se esperar para que o respawn ocorra
            /// </summary>
            public float Time
            {
                get
                {
                    return time;
                }
            }
        }

        private FrmMenu menu;
        private AccurateTimer timer;
        private float engineTime;
        private Random random;
        private Bomberman[] player;
        private List<HardBlock> blocks;
        private List<Sprite> sprites;
        private List<Sprite> addedSprites;
        private List<Sprite> removedSprites;
        private List<RespawnEntry> scheduleRespawns;
        private ImageList[,] creepImages;
        private ImageList[,] cactusImages;
        private ImageList[] softBlockImages;
        private int currentLevel;
        private int enemyCount;
        private bool changeLevel;
        private int levelToChange;
        private List<Entity> drawList;
        private Partition<Entity> partitionStatics;
        private Partition<Sprite> partitionSprites;
        private List<Vector2D> debugPath;
        private Graph2D graph;
        private Graph2D hardGraph;
        private float nextGenerateGraphNodeValuesThink;
        private int[] powerUpCount;
        private Vector2D drawOrigin;
        private float drawScale;
        private Bitmap background;
        private bool gameOver;
        private Bitmap gameOverImage;
        private Bitmap pressEnterImage;
        private float nextPressEnterToggleThink;
        private bool showPressEnter;
        private bool loadingLevel;
        private float nextGameOverThink;
        private KeyBinding keyBinding;
        private bool paused;
        private Box2D gameOverBox;
        private Box2D pressEnterBox;
        private SoundCollection sounds;
        private string currentStageMusic;
        private long lastCurrentMemoryUsage;

        private bool netplay;
        private bool netplayServer;
        private string netplayHost;
        private int netplayPort;
        private UdpClient udp;
        private MemoryStream stream;
        private BinaryReader reader;
        private BinaryWriter writter;

        public FrmBomberman(FrmMenu menu, KeyBinding keyBinding, SoundCollection sounds)
        {
            this.menu = menu;
            this.keyBinding = keyBinding;
            this.sounds = sounds;

            InitializeComponent();
        }

        public KeyBinding Binding
        {
            get
            {
                return keyBinding;
            }
            set
            {
                keyBinding = value;
            }
        }

        public static Graph2DCoords GetGraphCoords(Box2D box)
        {
            Vector2D mins = box.Origin + box.Mins;

            int col = (int) (mins.X / CELL_SIZE - INTERNAL_ORIGIN_COL);
            int row = (int) (mins.Y / CELL_SIZE - INTERNAL_ORIGIN_ROW);

            if (col >= 0 && col < INTERNAL_COL_COUNT && row >= 0 && row < INTERNAL_ROW_COUNT)
                return new Graph2DCoords(row, col);

            return null;
        }

        private void GenerateGraphNodeValues(Graph2DCoords coords)
        {
            GenerateGraphNodeValues(coords.Row, coords.Col);
        }

        private void GenerateGraphNodeValues(int row, int col)
        {
            if (engineTime >= nextGenerateGraphNodeValuesThink)
            {
                nextGenerateGraphNodeValuesThink = engineTime + GENERATE_GRAPH_NODE_VALUES_THINK_TIME;
                graph.GenerateNodeValues(row, col);

                if (DEBUG_GRAPH)
                    Invalidate();
            }
        }

        public Point TransformVector(Vector2D v)
        {
            return (v * drawScale + drawOrigin).ToPoint();
        }

        public Point TransformVector(float x, float y)
        {
            return TransformVector(new Vector2D(x, y));
        }

        public Vector2D TransformPoint(Point p)
        {
            return (new Vector2D(p) - drawOrigin) / drawScale;
        }

        public Vector2D TransformPoint(PointF p)
        {
            return (new Vector2D(p) - drawOrigin) / drawScale;
        }

        public Rectangle TransformBox(Box2D box)
        {
            return (box * drawScale + drawOrigin).ToRectangle();
        }

        public Box2D TransformRectangle(Rectangle rect)
        {
            return (new Box2D(rect) - drawOrigin) / drawScale;
        }

        public Box2D TransformRectangle(RectangleF rect)
        {
            return (new Box2D(rect) - drawOrigin) / drawScale;
        }

        public static Box2D GetBoxFromCell(int row, int col)
        {
            return GetBoxFromCell(row, col, CELL_SIZE, CELL_SIZE);
        }

        public static Box2D GetBoxFromCell(int row, int col, float width, float height)
        {
            float x = (col + INTERNAL_ORIGIN_COL) * CELL_SIZE;
            float y = (row + INTERNAL_ORIGIN_ROW) * CELL_SIZE;
            return new Box2D(new Vector2D(x, y), Vector2D.NULL_VECTOR, new Vector2D(width, height));
        }

        public List<Entity> GetOverlappedEntities(Box2D box, Sprite exclude = null, bool includeAdded = false, bool onlyStatics = false)
        {
            List<Entity> result = new List<Entity>();

            foreach (HardBlock block in blocks)
            {
                Box2D intersection = box & block.CollisionBox;

                if (intersection.Area() != 0)
                    result.Add(block);
            }

            if (includeAdded)
                foreach (Sprite sprite in addedSprites)
                {
                    if (sprite == exclude)
                        continue;

                    if (onlyStatics && !sprite.Static)
                        continue;

                    Box2D intersection = box & sprite.CollisionBox;

                    if (intersection.Area() != 0)
                        result.Add(sprite);
                }

            foreach (Sprite sprite in sprites)
            {
                if (sprite == exclude)
                    continue;

                if (onlyStatics && !sprite.Static)
                    continue;

                Box2D intersection = box & sprite.CollisionBox;

                if (intersection.Area() != 0)
                    result.Add(sprite);
            }

            return result;
        }

        public delegate bool OverlappingFilter(Entity entity);

        public static bool IsStatic(Entity entity)
        {
            return entity is HardBlock || entity is Sprite && ((Sprite) entity).Static;
        }

        public bool IsOverlapping(Box2D box, Sprite exclude = null, bool includeAdded = false, bool onlyStatics = false)
        {
            if (onlyStatics)
                return IsOverlapping(box, exclude, IsStatic, includeAdded);

            return IsOverlapping(box, exclude, null, includeAdded);
        }

        public bool IsOverlapping(Box2D box, Sprite exclude = null, OverlappingFilter filter = null, bool includeAdded = false)
        {
            foreach (HardBlock block in blocks)
            {
                if (filter != null && !filter(block))
                    continue;

                Box2D intersection = box & block.CollisionBox;

                if (intersection.Area() != 0)
                    return true;
            }

            if (includeAdded)
                foreach (Sprite sprite in addedSprites)
                {
                    if (sprite == exclude)
                        continue;

                    if (filter != null && !filter(sprite))
                        continue;

                    Box2D intersection = box & sprite.CollisionBox;

                    if (intersection.Area() != 0)
                        return true;
                }

            foreach (Sprite sprite in sprites)
            {
                if (sprite == exclude)
                    continue;

                if (filter != null && !filter(sprite))
                    continue;

                Box2D intersection = box & sprite.CollisionBox;

                if (intersection.Area() != 0)
                    return true;
            }

            return false;
        }

        private HardBlock MakeHardBlock(string name, int skin, int row, int col)
        {
            return MakeHardBlock(name, skin, row, col, CELL_SIZE, CELL_SIZE);
        }

        private HardBlock MakeHardBlock(string name, int skin, int row, int col, float width, float height)
        {
            return MakeHardBlock(name, GetBoxFromCell(row, col, width, height), skin);
        }

        private HardBlock MakeHardBlock(string name, Box2D box, int skin)
        {
            HardBlock block = new HardBlock(this, name, box, new ImageList[] { ilBlocks }, skin);

            partitionStatics.Insert(block);

            block.Spawn();
            blocks.Add(block);

            Graph2DCoords node = GetGraphCoords(block.CollisionBox);

            if (node != null)
            {
                graph.Delete(node);
                hardGraph.Delete(node);

                if (DEBUG_GRAPH)
                    Invalidate();
            }

            return block;
        }

        public static bool IsStaticOrBomb(Entity entity)
        {
            return entity is HardBlock || entity is Sprite && ((Sprite) entity).Static || entity is Bomb;
        }

        private Bomb SpawnBomb(Bomberman bomberman)
        {
            Box2D box = bomberman.CollisionBox;
            Vector2D m = box.Origin + box.Mins;

            Box2D cellBox1 = new Box2D(new Vector2D((int) (m.X / CELL_SIZE) * CELL_SIZE, (int) (m.Y / CELL_SIZE) * CELL_SIZE), Vector2D.NULL_VECTOR, new Vector2D(box.Size));
            Box2D intersection = cellBox1 & box;
            float area1 = intersection.Area();

            Box2D cellBox2 = cellBox1 + (box.Origin - cellBox1.Origin).Versor() * CELL_SIZE;
            intersection = cellBox2 & box;
            float area2 = intersection.Area();

            Box2D cellBox = area2 >= area1 ? cellBox2 : cellBox1;

            if (IsOverlapping(cellBox, bomberman, IsStaticOrBomb, true))
                return null;

            Bomb bomb = new Bomb(this, bomberman, cellBox, bomberman.RemoteControl ? (bomberman.RedBomb ? ilRemoteControlRedBomb : ilRemoteControlBomb) : (bomberman.RedBomb ? ilRedBomb : ilBomb));
            bomb.Spawn();

            Graph2DCoords node = GetGraphCoords(bomb.CollisionBox);

            if (node != null)
            {
                graph.Delete(node);

                if (DEBUG_GRAPH)
                    Invalidate();
            }

            return bomb;
        }

        private void SpawnFlames(Bomb bomb, int range)
        {
            Flame flame = new Flame(this, bomb.Bomberman, bomb.CollisionBox, ilFlame);
            flame.Spawn();

            int dx = 1;

            for (; dx <= range && SpawnFlame(bomb, dx, 0) != null; dx++) { }

            dx = -1;

            for (; dx >= -range && SpawnFlame(bomb, dx, 0) != null; dx--) { }

            int dy = 1;

            for (; dy <= range && SpawnFlame(bomb, 0, dy) != null; dy++) { }

            dy = -1;

            for (; dy >= -range && SpawnFlame(bomb, 0, dy) != null; dy--) { }
        }

        private Flame SpawnFlame(Bomb bomb, int dx, int dy)
        {
            Box2D box = bomb.CollisionBox + CELL_SIZE * dx * Vector2D.RIGHT_VECTOR + CELL_SIZE * dy * Vector2D.DOWN_VECTOR;

            foreach (Entity wall in blocks)
            {
                Box2D intersection = box & wall.CollisionBox;

                if (intersection.Area() != 0)
                    return null;
            }

            List<Sprite> spritesToHurt = new List<Sprite>();

            foreach (Sprite sprite in sprites)
            {
                if (sprite.MarkedToRemove)
                    continue;

                Box2D intersection = box & sprite.CollisionBox;

                if (intersection.Area() != 0)
                {
                    if (sprite is Flame)
                        continue;

                    if (sprite is Bomb)
                    {
                        ((Bomb) sprite).Explode();
                        return null;
                    }

                    if (sprite is SoftBlock)
                    {
                        sprite.Break(bomb.Bomberman);

                        if (!bomb.Red)
                            return null;
                    }
                    else
                        spritesToHurt.Add(sprite);
                }
            }

            Flame flame = new Flame(this, bomb.Bomberman, box, ilFlame);
            flame.Spawn();

            foreach (Sprite sprite in spritesToHurt)
                flame.Hurt(sprite, bomb.Bomberman, DEFAULT_HEALTH);

            return flame;
        }

        public PowerUp CreatePowerUp(PowerUpType type, Box2D box, bool spawn = true)
        {
            PowerUp powerup = new PowerUp(this, type, box, ilPowerUp);

            if (spawn)
                powerup.Spawn();

            return powerup;
        }

        private void SpawnPlayer1()
        {
            SpawnPlayer(PlayerNumber.FIRST);
        }

        private void SpawnPlayer2()
        {
            SpawnPlayer(PlayerNumber.SECOND);
        }

        private void SpawnPlayer(PlayerNumber number)
        {
            int index = PlayerNumberToInteger(number);
            player[index] = new Bomberman(this, "Bomberman P" + index, number, GetBoxFromCell(0, 0, CELL_SIZE, 2 * CELL_SIZE), new ImageList[] { ilBombermanWalkLeft, ilBombermanWalkUp, ilBombermanWalkRight, ilBombermanWalkDown, ilBombermanDeath, ilBombermanKickLeft, ilBombermanKickUp, ilBombermanKickRight, ilBombermanKickDown });
            player[index].Spawn();
        }

        private int RandomInt()
        {
            return random.Next();
        }

        private int RandomInt(int max)
        {
            return random.Next(max);
        }

        private int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        private float RandomFloat()
        {
            return (float) random.NextDouble();
        }

        public float GetEngineTime()
        {
            return engineTime;
        }

        private void TimerTick1()
        {
            if (timer.IsRunning)
                OnFrame();
            else
                Close();
        }

        public void ScheduleRespawn(Bomberman bomberman)
        {
            ScheduleRespawn(bomberman, RESPAWN_TIME);
        }

        public void ScheduleRespawn(Bomberman bomberman, float time)
        {
            scheduleRespawns.Add(new RespawnEntry(bomberman, GetEngineTime() + time));
        }

        public void NextLevel()
        {
            changeLevel = true;
            levelToChange = currentLevel + 1;
        }

        public PowerUpType RandomPowerUp()
        {
            float rnd = RandomFloat() * 100;
            float sum = POWERUP_ODD_DISTRIBUTION[0];

            if (rnd < sum)
                return PowerUpType.BOMB_PASS;

            for (int i = 1; i < POWERUP_ODD_DISTRIBUTION.Length; i++)
            {
                sum += POWERUP_ODD_DISTRIBUTION[i];

                if (rnd < sum)
                    return (PowerUpType) i;
            }

            return PowerUpType.VEST;
        }

        public void LoadLevel(int level)
        {
            paused = false;
            loadingLevel = true;

            UnloadLevel();

            currentStageMusic = "stage" + (level % STAGE_COUNT + 1);

            currentLevel = level;

            graph.Fetch();
            hardGraph.Fetch();

            background = new Bitmap((int) DEFAULT_GAME_AREA_RECT.Width, (int) DEFAULT_GAME_AREA_RECT.Height);
            Graphics g = Graphics.FromImage(background);
            using (TextureBrush brush = new TextureBrush(ilBackground.Images[currentLevel % STAGE_COUNT], WrapMode.Tile))
            {
                g.FillRectangle(brush, DEFAULT_GAME_AREA_RECT);
            }

            if (DEBUG_GRAPH)
                Invalidate();

            MakeHardBlock("Top block", currentLevel % STAGE_COUNT, -1, -1, COL_COUNT * CELL_SIZE, CELL_SIZE);
            MakeHardBlock("Bottom block", currentLevel % STAGE_COUNT, INTERNAL_ROW_COUNT, -1, COL_COUNT * CELL_SIZE, CELL_SIZE);
            MakeHardBlock("Left block", currentLevel % STAGE_COUNT, 0, -1, CELL_SIZE, INTERNAL_ROW_COUNT * CELL_SIZE);
            MakeHardBlock("Right block", currentLevel % STAGE_COUNT, 0, INTERNAL_COL_COUNT, CELL_SIZE, INTERNAL_ROW_COUNT * CELL_SIZE);

            for (int i = 0; i < (INTERNAL_COL_COUNT - 1) / 2; i++)
                for (int j = 0; j < (INTERNAL_ROW_COUNT - 1) / 2; j++)
                {
                    int row = 2 * j + 1;
                    int col = 2 * i + 1;
                    MakeHardBlock(("Middle block " + i) + j, currentLevel % STAGE_COUNT, row, col);
                }

            int randomHardBlockCount = 0;

            for (int row = 1; row < INTERNAL_ROW_COUNT - 1; row++)
            {
                if (RandomInt() % 2 == 1)
                    continue;

                Box2D box;
                int col;

                do
                {
                    col = RandomInt(INTERNAL_COL_COUNT - 2) + 1;
                    box = GetBoxFromCell(row, col);
                }
                while ((row + col) % 2 == 0 || IsOverlapping(box, null, true, false));

                MakeHardBlock("Middle block random " + randomHardBlockCount++, currentLevel % STAGE_COUNT, row, col);
            }

            for (int i = 0; i < powerUpCount.Length; i++)
                powerUpCount[i] = 0;

            List<int> powerUpLocations = new List<int>();
            PowerUpType[] powerUpTypes = new PowerUpType[MAX_SOFT_BLOCK_POWERUPS_PER_LEVEL];

            for (int i = 0; i < MAX_SOFT_BLOCK_POWERUPS_PER_LEVEL; i++)
            {
                int location = RandomInt(SOFT_BLOCK_COUNT);

                while (powerUpLocations.Contains(location))
                    location = RandomInt(SOFT_BLOCK_COUNT);

                powerUpLocations.Add(location);

                PowerUpType type = RandomPowerUp();
                int iType = (int) type;

                while (powerUpCount[iType] >= MAX_POWERUP_PER_LEVEL[currentLevel % STAGE_COUNT, iType])
                {
                    type = RandomPowerUp();
                    iType = (int) type;
                }

                powerUpCount[iType]++;
                powerUpTypes[i] = type;
            }

            int portalIndex = RandomInt(SOFT_BLOCK_COUNT);

            while (powerUpLocations.Contains(portalIndex))
                portalIndex = RandomInt(SOFT_BLOCK_COUNT);

            int powerUpCounter = 0;

            for (int i = 0; i < SOFT_BLOCK_COUNT; i++)
            {
                Box2D box;
                int col;
                int row;

                do
                {
                    col = RandomInt(INTERNAL_COL_COUNT);
                    row = RandomInt(INTERNAL_ROW_COUNT);
                    box = GetBoxFromCell(row, col);
                }
                while (col == 0 && row == 0 || col == 0 && row == 1 || col == 1 && row == 0 || IsOverlapping(box, null, true, false));

                Sprite prize = null;

                if (i == portalIndex)
                    prize = new Portal(this, box, ilPortal);
                else if (powerUpCounter < MAX_SOFT_BLOCK_POWERUPS_PER_LEVEL && powerUpLocations.Contains(i))
                    prize = CreatePowerUp(powerUpTypes[powerUpCounter++], box, false);

                SoftBlock block = new SoftBlock(this, box, prize, softBlockImages[currentLevel % STAGE_COUNT]);
                block.Spawn();

                Graph2DCoords node = GetGraphCoords(block.CollisionBox);

                if (node != null)
                {
                    graph.Delete(node);

                    if (DEBUG_GRAPH)
                        Invalidate();
                }
            }

            int creepCount = 5 + level / 2;

            if (creepCount > MAX_CREEPS)
                creepCount = MAX_CREEPS;

            int cactusCount = level;

            if (cactusCount > MAX_CACTUS)
                cactusCount = MAX_CACTUS;

            for (int i = 0; i < creepCount + cactusCount; i++)
            {
                Box2D box;
                int row;
                int col;

                do
                {
                    col = RandomInt(INTERNAL_COL_COUNT);
                    row = RandomInt(INTERNAL_ROW_COUNT);
                    box = GetBoxFromCell(row, col);
                }
                while (col <= 3 && row <= 3 || IsOverlapping(box, null, true, false));

                if (i < creepCount)
                {
                    Creep creep = new Creep(this, "Creep", CREEP_POINTS, box, new ImageList[] { creepImages[currentLevel % CREEP_IMAGE_COUNT, 0], creepImages[currentLevel % CREEP_IMAGE_COUNT, 1], creepImages[currentLevel % CREEP_IMAGE_COUNT, 2], creepImages[currentLevel % CREEP_IMAGE_COUNT, 3] });
                    creep.Spawn();
                }
                else
                {
                    Cactus cactus = new Cactus(this, "Cactus", CACTUS_POINTS, box, new ImageList[] { cactusImages[currentLevel % CACTUS_IMAGE_COUNT, 0], cactusImages[currentLevel % CACTUS_IMAGE_COUNT, 1], cactusImages[currentLevel % CACTUS_IMAGE_COUNT, 2], cactusImages[currentLevel % CACTUS_IMAGE_COUNT, 3] });
                    cactus.Spawn();
                }
            }

            Bomberman oldBolberman = player[0];
            SpawnPlayer1();

            if (oldBolberman != null)
            {
                player[0].Lives = oldBolberman.Lives;
                player[0].Range = oldBolberman.Range;
                player[0].Bombs = oldBolberman.Bombs;
                player[0].RedBomb = oldBolberman.RedBomb;
                player[0].Speed = oldBolberman.Speed;
                player[0].PassBombs = oldBolberman.PassBombs;
                player[0].PassStatics = oldBolberman.PassStatics;
                player[0].Health = oldBolberman.Health;
                player[0].RemoteControl = oldBolberman.RemoteControl;
                player[0].KickBombs = oldBolberman.KickBombs;
                player[0].Score = oldBolberman.Score;
            }

            loadingLevel = false;

            PlaySound(currentStageMusic, true);

            Invalidate();
        }

        private void UnloadLevel()
        {
            foreach (HardBlock block in blocks)
                block.Dispose();

            foreach (Sprite sprite in addedSprites)
                sprite.Dispose();

            foreach (Sprite sprite in sprites)
                sprite.Dispose();

            graph.Clear();
            hardGraph.Clear();
            blocks.Clear();
            sprites.Clear();
            addedSprites.Clear();
            removedSprites.Clear();
            scheduleRespawns.Clear();
            partitionStatics.Clear();
            partitionSprites.Clear();

            if (background != null)
            {
                background.Dispose();
                background = null;
            }

            if (currentStageMusic != "")
            {
                StopSound(currentStageMusic);
                currentStageMusic = "";
            }

            long currentMemoryUsage = GC.GetTotalMemory(true);
            long delta = currentMemoryUsage - lastCurrentMemoryUsage;
            Debug.WriteLine("**************************Total memory: {0}({1}{2})", currentMemoryUsage, delta > 0 ? "+" : delta < 0 ? "-" : "", delta);
            lastCurrentMemoryUsage = currentMemoryUsage;
        }

        private void frmBomberman_Load(object sender, EventArgs e)
        {
            loadingLevel = true;

            player = new Bomberman[2];

            drawOrigin = DEFAULT_DRAW_ORIGIN;
            drawScale = DEFAULT_DRAW_SCALE;

            gameOverImage = null;
            pressEnterImage = null;
            showPressEnter = false;
            gameOverBox = Box2D.EMPTY_BOX;
            pressEnterBox = Box2D.EMPTY_BOX;

            graph = new Graph2D(INTERNAL_ROW_COUNT, INTERNAL_COL_COUNT);
            hardGraph = new Graph2D(INTERNAL_ROW_COUNT, INTERNAL_COL_COUNT);
            nextGenerateGraphNodeValuesThink = 0;
            debugPath = new List<Vector2D>();

            creepImages = new ImageList[CREEP_IMAGE_COUNT, 4];
            cactusImages = new ImageList[CACTUS_IMAGE_COUNT, 4];
            softBlockImages = new ImageList[STAGE_COUNT];

            powerUpCount = new int[POWER_UP_COUNT];

            creepImages[0, 0] = ilCreep00Left;
            creepImages[0, 1] = ilCreep00Up;
            creepImages[0, 2] = ilCreep00Right;
            creepImages[0, 3] = ilCreep00Down;

            creepImages[1, 0] = ilCreep01Left;
            creepImages[1, 1] = ilCreep01Up;
            creepImages[1, 2] = ilCreep01Right;
            creepImages[1, 3] = ilCreep01Down;

            creepImages[2, 0] = ilCreep02Left;
            creepImages[2, 1] = ilCreep02Up;
            creepImages[2, 2] = ilCreep02Right;
            creepImages[2, 3] = ilCreep02Down;

            cactusImages[0, 0] = ilCactus00Left;
            cactusImages[0, 1] = ilCactus00Up;
            cactusImages[0, 2] = ilCactus00Right;
            cactusImages[0, 3] = ilCactus00Down;

            cactusImages[1, 0] = ilCactus01Left;
            cactusImages[1, 1] = ilCactus01Up;
            cactusImages[1, 2] = ilCactus01Right;
            cactusImages[1, 3] = ilCactus01Down;

            cactusImages[2, 0] = ilCactus02Left;
            cactusImages[2, 1] = ilCactus02Up;
            cactusImages[2, 2] = ilCactus02Right;
            cactusImages[2, 3] = ilCactus02Down;

            softBlockImages[0] = ilSoftBlock00;
            softBlockImages[1] = ilSoftBlock01;
            softBlockImages[2] = ilSoftBlock02;
            softBlockImages[3] = ilSoftBlock03;
            softBlockImages[4] = ilSoftBlock04;
            softBlockImages[5] = ilSoftBlock05;
            softBlockImages[6] = ilSoftBlock06;

            timer = new AccurateTimer(this, new Action(TimerTick1), (int) (1000 * TICK));
            engineTime = 0;

            random = new Random();
            blocks = new List<HardBlock>();
            sprites = new List<Sprite>();
            addedSprites = new List<Sprite>();
            removedSprites = new List<Sprite>();
            scheduleRespawns = new List<RespawnEntry>();

            changeLevel = false;

            drawList = new List<Entity>();

            partitionStatics = new Partition<Entity>(DEFAULT_CLIENT_RECT, ROW_COUNT, COL_COUNT);
            partitionSprites = new Partition<Sprite>(DEFAULT_CLIENT_RECT, ROW_COUNT, COL_COUNT);

            background = null;

            ClientSize = new Size((int) DEFAULT_CLIENT_WIDTH, (int) DEFAULT_CLIENT_HEIGHT);
            UpdateScale();

            currentStageMusic = "";

            LoadLevel(INITIAL_LEVEL);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (timer.IsRunning)
            {
                timer.Stop();

                UnloadLevel();
                StopSound("gameover");

                if (gameOverImage != null)
                {
                    gameOverImage.Dispose();
                    gameOverImage = null;
                }

                if (pressEnterImage != null)
                {
                    pressEnterImage.Dispose();
                    pressEnterImage = null;
                }

                if (menu != null)
                {
                    menu.Show();

                    if (gameOver)
                        menu.ReportScore(currentLevel, player[0].Score);
                }

                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        public void PlaySound(string soundName, bool loop = false)
        {
            if (sounds != null)
                sounds.Play(soundName, loop);
        }

        public void StopSound(string soundName)
        {
            if (sounds != null)
                sounds.Stop(soundName);
        }

        public void TogglePauseGame()
        {
            if (paused)
                ContinueGame();
            else
                PauseGame();
        }

        public void PauseGame()
        {
            paused = true;
            PlaySound("pause");
            Invalidate();
        }

        public void ContinueGame()
        {
            paused = false;
            Invalidate();
        }

        private void frmBomberman_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;

            if (gameOver)
            {
                if (gameOverImage != null && keyCode == Keys.Return)
                {
                    PlaySound("confirm");
                    Close();
                }

                return;
            }

            if (!paused)
            {
                if (keyCode == keyBinding.Left)
                    player[0].Keys |= DirectionToInt(Direction.LEFT);
                else if (keyCode == keyBinding.Up)
                    player[0].Keys |= DirectionToInt(Direction.UP);
                else if (keyCode == keyBinding.Right)
                    player[0].Keys |= DirectionToInt(Direction.RIGHT);
                else if (keyCode == keyBinding.Down)
                    player[0].Keys |= DirectionToInt(Direction.DOWN);
                else if (keyCode == keyBinding.DropBomb)
                    player[0].DropBomb();
                else if (keyCode == keyBinding.Kick)
                    player[0].Kick();
                else if (keyCode == keyBinding.Detonate)
                    player[0].Detonate();
                else if (keyCode == keyBinding.Pause)
                    PauseGame();
            }
            else if (keyCode == keyBinding.Pause)
                ContinueGame();
        }

        private void frmBomberman_KeyUp(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;

            if (keyCode == keyBinding.Left)
                player[0].Keys &= ~DirectionToInt(Direction.LEFT);
            else if (keyCode == keyBinding.Up)
                player[0].Keys &= ~DirectionToInt(Direction.UP);
            else if (keyCode == keyBinding.Right)
                player[0].Keys &= ~DirectionToInt(Direction.RIGHT);
            else if (keyCode == keyBinding.Down)
                player[0].Keys &= ~DirectionToInt(Direction.DOWN);
        }

        private void Repaint(Entity entity)
        {
            if (!drawList.Contains(entity))
                drawList.Add(entity);
        }

        private void OnFrame()
        {
            if (!paused && !loadingLevel)
            {
                engineTime += TICK;

                int count = scheduleRespawns.Count;

                for (int i = 0; i < count; i++)
                {
                    RespawnEntry entry = scheduleRespawns[i];

                    if (GetEngineTime() >= entry.Time)
                    {
                        scheduleRespawns.RemoveAt(i);
                        i--;
                        count--;

                        string name = entry.Bomberman.Name;
                        Box2D spawnBox = entry.Bomberman.SpawnBox;
                        int lives = entry.Bomberman.Lives;
                        int range = entry.Bomberman.Range;
                        int bombs = entry.Bomberman.Bombs;
                        int score = entry.Bomberman.Score;

                        SpawnPlayer1();
                        player[0].Lives = lives;
                        player[0].Range = range;
                        player[0].Bombs = bombs;
                        player[0].Score = score;
                    }
                }

                if (addedSprites.Count > 0)
                {
                    foreach (Sprite added in addedSprites)
                    {
                        sprites.Add(added);

                        if (added.Static)
                            partitionStatics.Insert(added);
                        else
                            partitionSprites.Insert(added);

                        added.OnAdded(sprites.Count - 1);
                        Repaint(added);
                    }

                    addedSprites.Clear();
                }

                foreach (Sprite sprite in sprites)
                {
                    if (changeLevel)
                        break;

                    if (!sprite.MarkedToRemove)
                        sprite.OnFrame();
                }

                if (removedSprites.Count > 0)
                {
                    foreach (Sprite removed in removedSprites)
                    {
                        sprites.Remove(removed);

                        if (removed.Static)
                            partitionStatics.Remove(removed);
                        else
                            partitionSprites.Remove(removed);

                        Repaint(removed);
                    }

                    removedSprites.Clear();
                }
            }

            if (changeLevel)
            {
                changeLevel = false;
                LoadLevel(levelToChange);
            }

            foreach (Entity entity in drawList)
            {
                Box2D drawBox = entity.DrawBox;
                Vector2D delta = entity.LastDelta;

                if (delta != Vector2D.NULL_VECTOR)
                {
                    Box2D lastBox = drawBox - delta;
                    Box2D intersection = lastBox & drawBox;

                    if (intersection.Area() != 0)
                    {
                        Box2D union = lastBox | drawBox;
                        Invalidate(TransformBox(union), false);
                        Update();
                    }
                    else
                    {
                        Invalidate(TransformBox(lastBox), false);
                        Invalidate(TransformBox(drawBox), false);
                        Update();
                    }
                }
                else
                {
                    Invalidate(TransformBox(drawBox), false);
                    Update();
                }
            }

            drawList.Clear();

            if (gameOver)
            {
                if (gameOverImage == null && engineTime >= nextGameOverThink)
                {
                    if (currentStageMusic != "")
                    {
                        StopSound(currentStageMusic);
                        currentStageMusic = "";
                    }

                    PlaySound("gameover");

                    gameOverImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.GameOver.game_over.png"));
                    Vector2D imageSize = new Vector2D(gameOverImage.Size);
                    gameOverBox = new Box2D(Vector2D.NULL_VECTOR, Vector2D.NULL_VECTOR, imageSize) + (DEFAULT_CLIENT_BOX.SizeVector - imageSize) / 2;

                    pressEnterImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Bomberman.Resources.Textures.GameOver.press_enter.png"));
                    pressEnterBox = new Box2D(PRESS_ENTER_COORDS, Vector2D.NULL_VECTOR, new Vector2D(pressEnterImage.Size)) + gameOverBox.LeftTop;
                    nextPressEnterToggleThink = engineTime + PRESS_ENTER_FLASH_TIME;
                    showPressEnter = false;

                    Invalidate(gameOverBox.ToRectangle(), false);
                }
                else if (engineTime >= nextPressEnterToggleThink)
                {
                    nextPressEnterToggleThink = engineTime + PRESS_ENTER_FLASH_TIME;
                    showPressEnter = !showPressEnter;
                    Invalidate(TransformBox(pressEnterBox), false);
                }
            }
        }

        private static readonly Vector2D SHIFT_CENTER = new Vector2D(CELL_SIZE / 2, CELL_SIZE / 2);

        public void SetDebugPath(Box2D box, List<Direction> route)
        {
            debugPath.Clear();
            debugPath.Add(box.Origin + SHIFT_CENTER);

            foreach (Direction direction in route)
            {
                box += GetVectorDir(direction) * CELL_SIZE;
                debugPath.Add(box.Origin + SHIFT_CENTER);
            }

            Invalidate();
        }

        private void UpdateScale()
        {
            float width = ClientRectangle.Width;
            float height = ClientRectangle.Height;

            if (width / height < SIZE_RATIO)
            {
                drawScale = width / DEFAULT_CLIENT_WIDTH;
                float newHeight = drawScale * DEFAULT_CLIENT_HEIGHT;
                drawOrigin = new Vector2D(0, (height - newHeight) / 2);
            }
            else
            {
                drawScale = height / DEFAULT_CLIENT_HEIGHT;
                float newWidth = drawScale * DEFAULT_CLIENT_WIDTH;
                drawOrigin = new Vector2D((width - newWidth) / 2, 0);
            }
        }

        private void frmBomberman_SizeChanged(object sender, EventArgs e)
        {
            UpdateScale();
            Invalidate();
        }

        public void RepaintLevel()
        {
            Invalidate(TransformBox(DEFAULT_LEVEL_BOX), false);
            Update();
        }

        public void RepaintLives()
        {
            Invalidate(TransformBox(DEFAULT_LIVES_BOX), false);
            Update();
        }

        private void RepaintScore()
        {
            Invalidate(TransformBox(DEFAULT_SCORE_BOX), false);
            Update();
        }

        private void RepaintTime()
        {
            Invalidate(TransformBox(DEFAULT_TIME_BOX), false);
            Update();
        }

        private void RepaintBombs()
        {
            Invalidate(TransformBox(DEFAULT_BOMBS_BOX), false);
            Update();
        }

        private void RepaintFire()
        {
            Invalidate(TransformBox(DEFAULT_FIRE_BOX), false);
            Update();
        }

        private void RepaintHeart()
        {
            Invalidate(TransformBox(DEFAULT_HEART_BOX), false);
            Update();
        }

        private void frmBomberman_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle clipRect = e.ClipRectangle;
            Box2D drawBox = TransformRectangle(clipRect); // Obtém a draw box já com a transformação escalar aplicada

            // Desenha o top panel
            if ((DEFAULT_TOP_PANEL_BOX & drawBox).Area() != 0)
            {
                Font font = new Font("Arial", 30 * drawScale, GraphicsUnit.Pixel);

                // Desenha o número do level
                if ((DEFAULT_LEVEL_BOX & drawBox).Area() != 0)
                {
                    string text = "Lv " + (currentLevel + 1);
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(text, font, brush, TransformVector(DEFAULT_LEVEL_BOX.LeftTop + (DEFAULT_LEVEL_BOX.SizeVector - fontSize) / 2));
                    }
                }

                // Desenha a quantidade de vidas
                if ((DEFAULT_LIVES_IMG_BOX & drawBox).Area() != 0)
                    g.DrawImage(ilTopPanel.Images[3], TransformBox(DEFAULT_LIVES_IMG_BOX));

                if ((DEFAULT_LIVES_BOX & drawBox).Area() != 0)
                {
                    string text = player[0].Lives.ToString();
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(text, font, brush, TransformVector(DEFAULT_LIVES_BOX.LeftTop + (DEFAULT_LIVES_BOX.SizeVector - fontSize) / 2));
                    }
                }

                // Desenha o score
                if ((DEFAULT_SCORE_BOX & drawBox).Area() != 0)
                {
                    using (Pen pen = new Pen(Color.White, 2))
                    {
                        g.DrawRectangle(pen, TransformBox(DEFAULT_SCORE_BOX));
                    }

                    string text = player[0].Score.ToString();
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        Vector2D lt = DEFAULT_SCORE_BOX.LeftTop;
                        float x = lt.X + DEFAULT_SCORE_BOX.Width - fontSize.X;
                        float y = lt.Y + (DEFAULT_SCORE_BOX.Height - fontSize.Y) / 2;
                        g.DrawString(text, font, brush, TransformVector(x, y));
                    }
                }

                // Desenha o tempo
                if ((DEFAULT_TIME_BOX & drawBox).Area() != 0)
                {
                    using (Pen pen = new Pen(Color.White, 2))
                    {
                        g.DrawRectangle(pen, TransformBox(DEFAULT_TIME_BOX));
                    }

                    int seconds = (int)player[0].Time;
                    int minutes = seconds / 60;
                    seconds = seconds % 60;
                    string text = minutes + ":" + (seconds < 10 ? "0" + seconds : seconds.ToString());
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        Vector2D lt = DEFAULT_TIME_BOX.LeftTop;
                        float x = lt.X + DEFAULT_TIME_BOX.Width - fontSize.X;
                        float y = lt.Y + (DEFAULT_TIME_BOX.Height - fontSize.Y) / 2;
                        g.DrawString(text, font, brush, TransformVector(x, y));
                    }
                }

                // Desenha a quantidade máxima de bombas
                if ((DEFAULT_BOMBS_IMG_BOX & drawBox).Area() != 0)
                    g.DrawImage(ilTopPanel.Images[0], TransformBox(DEFAULT_BOMBS_IMG_BOX));

                if ((DEFAULT_BOMBS_BOX & drawBox).Area() != 0)
                {
                    string text = player[0].Bombs.ToString();
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(text, font, brush, TransformVector(DEFAULT_BOMBS_BOX.LeftTop + (DEFAULT_BOMBS_BOX.SizeVector - fontSize) / 2));
                    }
                }

                // Desenha o range das bombas
                if ((DEFAULT_FIRE_IMG_BOX & drawBox).Area() != 0)
                    g.DrawImage(ilTopPanel.Images[1], TransformBox(DEFAULT_FIRE_IMG_BOX));

                if ((DEFAULT_FIRE_BOX & drawBox).Area() != 0)
                {
                    string text = player[0].Range.ToString();
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(text, font, brush, TransformVector(DEFAULT_FIRE_BOX.LeftTop + (DEFAULT_FIRE_BOX.SizeVector - fontSize) / 2));
                    }
                }

                // Desenha o número de corações
                if ((DEFAULT_HEART_IMG_BOX & drawBox).Area() != 0)
                    g.DrawImage(ilTopPanel.Images[2], TransformBox(DEFAULT_HEART_IMG_BOX));

                if ((DEFAULT_HEART_BOX & drawBox).Area() != 0)
                {
                    int hearts = (int) (player[0].Health / DEFAULT_HEALTH);
                    string text = (hearts > 1 ? hearts - 1 : 0).ToString();
                    Vector2D fontSize = new Vector2D(g.MeasureString(text, font)) / drawScale;
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(text, font, brush, TransformVector(DEFAULT_HEART_BOX.LeftTop + (DEFAULT_HEART_BOX.SizeVector - fontSize) / 2));
                    }
                }
            }

            // Desenha o fundo
            if (background != null && (DEFAULT_GAME_AREA_BOX & drawBox).Area() != 0)
                g.DrawImage(background, clipRect, drawBox.ToRectangle(), GraphicsUnit.Pixel);

            // Desenha os blocos
            List<Entity> statics = partitionStatics.Query(drawBox);

            foreach (Entity entity in statics)
                entity.Paint(g);

            // Desenha os sprites
            List<Sprite> sprites = partitionSprites.Query(drawBox);

            foreach (Sprite sprite in sprites)
                if (sprite != player[0] && !sprite.MarkedToRemove)
                    sprite.Paint(g);

            // Desenha o bomberman
            if ((player[0].DrawBox & drawBox).Area() != 0)
                player[0].Paint(g);

            // Game Over
            if (gameOverImage != null && (gameOverBox & drawBox).Area() != 0)
            {
                g.DrawImage(gameOverImage, TransformBox(gameOverBox));

                if (showPressEnter && (pressEnterBox & drawBox).Area() != 0)
                    g.DrawImage(pressEnterImage, TransformBox(pressEnterBox));
            }

            // Debug
            if (debugPath.Count > 0)
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    Vector2D v0 = debugPath[0];

                    for (int i = 1; i < debugPath.Count; i++)
                    {
                        Vector2D v = debugPath[i];
                        g.DrawLine(pen, drawOrigin.X + v0.X * drawScale, drawOrigin.Y + v0.Y * drawScale, drawOrigin.X + v.X * drawScale, drawOrigin.Y + v.Y * drawScale);
                        v0 = v;
                    }
                }

            if (DEBUG_GRAPH)
            {
                Font font = new Font("Arial", 16 * drawScale);
                using (Brush brush = new SolidBrush(Color.Blue))
                {
                    for (int row = 0; row < INTERNAL_ROW_COUNT; row++)
                        for (int col = 0; col < INTERNAL_COL_COUNT; col++)
                        {
                            Graph2D.Node node = graph[row, col];

                            if (node == null)
                                continue;

                            int value = node.Value;

                            float x = (col + INTERNAL_ORIGIN_COL) * CELL_SIZE;
                            float y = (row + INTERNAL_ORIGIN_ROW) * CELL_SIZE;
                            string text = value == Graph2D.INFINITE ? "∞" : value.ToString();

                            SizeF size = g.MeasureString(text, font);

                            g.DrawString(text, font, brush, drawOrigin.X + x + (CELL_SIZE - size.Width) / 2 * drawScale, drawOrigin.Y + y + (CELL_SIZE - size.Height) / 2 * drawScale);
                        }
                }
            }

            if (DEBUG_DRAW_CLIPRECT)
                using (Pen pen = new Pen(Color.Yellow, 2))
                {
                    g.DrawRectangle(pen, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height);
                }
        }

        private void OnGameOver()
        {
            gameOver = true;
            nextGameOverThink = engineTime + GAME_OVER_PANEL_SHOW_DELAY;
        }

        private bool IsAllPlayersDied()
        {
            for (int i = 0; i < 2; i++)
                if (player[i] != null && !player[i].IsDeath())
                    return false;

            return true;
        }

        private void SendStateOverNetwork()
        {
            stream.Position = 0;
            writter.Write((byte)sprites.Count);
            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite sprite = sprites[i];
                sprite.Write(writter);
            }

            byte[] bytes = stream.ToArray();
            udp.SendAsync(bytes, bytes.Length);
        }

        private void ReceiveStateFromNetwork()
        {
            System.Net.IPEndPoint remoteEP = null;
            byte[] receivedBytes = udp.Receive(ref remoteEP);
        }
    }
}