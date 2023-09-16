using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Views;
using Push;
using Push.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.TextureView;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Push.Droid
{
    public class CameraViewRenderer : ViewRenderer<CameraView, TextureView>
    {
        private CameraManager _cameraManager;
        private CameraDevice _cameraDevice;
        private CameraCaptureSession _captureSession;
        private CaptureRequest.Builder _captureRequestBuilder;
        private SurfaceTexture _surfaceTexture;
        private Size _previewSize;

        public CameraViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe from events and release resources if needed
                StopCameraPreview();
            }

            if (e.NewElement != null)
            {
                // Create and display the TextureView
                var textureView = new TextureView(Context);
                textureView.SurfaceTextureListener = new SurfaceTextureListener(this);
                SetNativeControl(textureView);
            }
        }
        /*
         * Ryan I belive the StartCameraPreview is where the camera opens.
         * Specifically this pary "_cameraManager.OpenCamera(cameraId, new CameraStateCallback(this), null);"
         * I am not positive but you may have to right-click on opencamera and go-to definition and modify the opencamera function. 
         * the issue is that is the built in function of the android camera so modifiying it could cause some serious problems.
         * Not sure what you will need but this may be a place to start
         */
        private void StartCameraPreview()
        {
            _cameraManager = (CameraManager)Context.GetSystemService(Context.CameraService);

            try
            {
                string cameraId = null;

                // Get a list of available camera IDs
                string[] cameraIds = _cameraManager.GetCameraIdList();

                // Loop through the camera IDs to find the front camera
                foreach (string id in cameraIds)
                {
                    CameraCharacteristics cameraCharacteristics = _cameraManager.GetCameraCharacteristics(id);
                    int facing = (int)cameraCharacteristics.Get(CameraCharacteristics.LensFacing);

                    if (facing == (int)LensFacing.Front)
                    {
                        // Front camera found, set the cameraId and break
                        cameraId = id;
                        break;
                    }
                }

                if (cameraId == null)
                {
                    // Handle the case where the front camera is not available
                    return;
                }

                // Get the characteristics of the selected camera
                var characteristics = _cameraManager.GetCameraCharacteristics(cameraId);
                var streamConfigurationMap = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

                if (streamConfigurationMap == null)
                {
                    // Handle the case where Camera2 API is not supported on the device
                    return;
                }

                // Choose a suitable preview size based on your requirements
                _previewSize = ChooseOptimalSize(streamConfigurationMap.GetOutputSizes((int)ImageFormatType.Yuv420888), Control.Width, Control.Height);

                _surfaceTexture = Control.SurfaceTexture;
                _surfaceTexture.SetDefaultBufferSize((int)_previewSize.Width, (int)_previewSize.Height);

                // Open the front camera
                _cameraManager.OpenCamera(cameraId, new CameraStateCallback(this), null);
                RunPushUpCounter.Main();
            }
            catch (CameraAccessException e)
            {
                // Handle camera access exception
            }
        }


        private Size ChooseOptimalSize(Android.Util.Size[] choices, int width, int height)
        {
            var desiredAspectRatio = (double)width / height;
            Android.Util.Size optimalSize = null;
            double minAspectRatioDiff = double.MaxValue;

            foreach (var choice in choices)
            {
                double aspectRatioDiff = Math.Abs((double)choice.Width / choice.Height - desiredAspectRatio);

                if (aspectRatioDiff < minAspectRatioDiff)
                {
                    optimalSize = choice;
                    minAspectRatioDiff = aspectRatioDiff;
                }
            }

            // Convert Android.Util.Size to Xamarin.Forms.Size
            return new Size(optimalSize.Width, optimalSize.Height);
        }

        private void StopCameraPreview()
        {
            if (_captureSession != null)
            {
                _captureSession.Close();
                _captureSession = null;
            }

            if (_cameraDevice != null)
            {
                _cameraDevice.Close();
                _cameraDevice = null;
            }
        }

       

        private class SurfaceTextureListener : Java.Lang.Object, ISurfaceTextureListener
        {
            private readonly CameraViewRenderer _renderer;

            public SurfaceTextureListener(CameraViewRenderer renderer)
            {
                _renderer = renderer;
            }

            public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
            {
                _renderer.StartCameraPreview();
            }

            public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
            {
                _renderer.StopCameraPreview();
                return true;
            }

            public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
            {
                // Handle any changes in the size of the TextureView or camera preview here
            }

            public void OnSurfaceTextureUpdated(SurfaceTexture surface)
            {
                // This method is called when the TextureView's surface is updated
            }
        }

        private class CameraStateCallback : CameraDevice.StateCallback
        {
            private readonly CameraViewRenderer _renderer;

            public CameraStateCallback(CameraViewRenderer renderer)
            {
                _renderer = renderer;
            }

            public override void OnOpened(CameraDevice camera)
            {
                _renderer._cameraDevice = camera;

                // Create a camera capture session
                var surfaces = new List<Surface> { new Surface(_renderer._surfaceTexture) };
                var previewRequestBuilder = camera.CreateCaptureRequest(CameraTemplate.Preview);
                previewRequestBuilder.AddTarget(new Surface(_renderer._surfaceTexture));

                try
                {
                    camera.CreateCaptureSession(surfaces, new CameraCaptureSessionCallback(_renderer), null);
                }
                catch (CameraAccessException e)
                {
                    // Handle camera access exception
                }
            }

            public override void OnDisconnected(CameraDevice camera)
            {
                camera.Close();
                _renderer._cameraDevice = null;
            }

            public override void OnError(CameraDevice camera, CameraError error)
            {
                camera.Close();
                _renderer._cameraDevice = null;
            }
        }

        private class CameraCaptureSessionCallback : CameraCaptureSession.StateCallback
        {
            private readonly CameraViewRenderer _renderer;

            public CameraCaptureSessionCallback(CameraViewRenderer renderer)
            {
                _renderer = renderer;
            }

            public override void OnConfigured(CameraCaptureSession session)
            {
                if (_renderer._cameraDevice == null)
                {
                    return;
                }

                _renderer._captureSession = session;

                try
                {
                    // Set the desired capture request parameters here, e.g., auto-focus, flash, etc.
                    _renderer._captureRequestBuilder = _renderer._cameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
                    _renderer._captureRequestBuilder.AddTarget(new Surface(_renderer._surfaceTexture));

                    session.SetRepeatingRequest(_renderer._captureRequestBuilder.Build(), null, null);
                }
                catch (CameraAccessException e)
                {
                    // Handle camera access exception
                }
            }

            public override void OnConfigureFailed(CameraCaptureSession session)
            {
                // Handle configuration failure
            }
        }
    }
}




