/*
 *  PluginFunctions.cs
 *  ARToolKit for Unity
 *
 *  This file is part of ARToolKit for Unity.
 *
 *  ARToolKit for Unity is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  ARToolKit for Unity is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with ARToolKit for Unity.  If not, see <http://www.gnu.org/licenses/>.
 *
 *  As a special exception, the copyright holders of this library give you
 *  permission to link this library with independent modules to produce an
 *  executable, regardless of the license terms of these independent modules, and to
 *  copy and distribute the resulting executable under terms of your choice,
 *  provided that you also meet, for each linked independent module, the terms and
 *  conditions of the license of that module. An independent module is a module
 *  which is neither derived from nor based on this library. If you modify this
 *  library, you may extend this exception to your version of the library, but you
 *  are not obligated to do so. If you do not wish to do so, delete this exception
 *  statement from your version.
 *
 *  Copyright 2015 Daqri, LLC.
 *  Copyright 2010-2015 ARToolworks, Inc.
 *
 *  Author(s): Philip Lamb, Julian Looser
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

public static class PluginFunctions
{
	[NonSerialized]
	public static bool inited = false;

	// Delegate type declaration.
	public delegate void LogCallback([MarshalAs(UnmanagedType.LPStr)] string msg);

	// Delegate instance.
	private static LogCallback logCallback = null;
	private static GCHandle logCallbackGCH;

	public static void arwRegisterLogCallback(LogCallback lcb)
	{
		if (lcb != null) {
			logCallback = lcb;
			logCallbackGCH = GCHandle.Alloc(logCallback); // Does not need to be pinned, see http://stackoverflow.com/a/19866119/316487 
		}
        #if UNITY_IPHONE
		ARNativePluginStatic.arwRegisterLogCallback(logCallback);
        #else
		ARNativePlugin.arwRegisterLogCallback(logCallback);
        #endif
		if (lcb == null) {
			logCallback = null;
			logCallbackGCH.Free();
		}
	}

	public static void arwSetLogLevel(int logLevel)
	{
        #if UNITY_IPHONE  		
        ARNativePluginStatic.arwSetLogLevel(logLevel);
        #else
		ARNativePlugin.arwSetLogLevel(logLevel);
        #endif
	}

	public static bool arwInitialiseAR(int pattSize = 16, int pattCountMax = 25)
	{
		bool ok;
        #if UNITY_IPHONE 
        ok = ARNativePluginStatic.arwInitialiseARWithOptions(pattSize, pattCountMax);
        #else
        ok = ARNativePlugin.arwInitialiseARWithOptions(pattSize, pattCountMax);
        #endif
		if (ok) PluginFunctions.inited = true;
		return ok;
	}
	
	public static string arwGetARToolKitVersion()
	{
		StringBuilder sb = new StringBuilder(128);
		bool ok;
        #if UNITY_IPHONE 
		ok = ARNativePluginStatic.arwGetARToolKitVersion(sb, sb.Capacity);
        #else 
		ok = ARNativePlugin.arwGetARToolKitVersion(sb, sb.Capacity);
        #endif
		if (ok) return sb.ToString();
		else return "unknown";
	}

	public static int arwGetError()
	{
        #if UNITY_IPHONE 
		return ARNativePluginStatic.arwGetError();
        #else 
		return ARNativePlugin.arwGetError();
        #endif
	}

    public static bool arwShutdownAR()
	{
		bool ok;
        #if UNITY_IPHONE 
		ok = ARNativePluginStatic.arwShutdownAR();
        #else
		ok = ARNativePlugin.arwShutdownAR(); 
        #endif
		if (ok) PluginFunctions.inited = false;
		return ok;
	}
	
	public static bool arwStartRunningB(string vconf, byte[] cparaBuff, int cparaBuffLen, float nearPlane, float farPlane)
	{
        #if UNITY_IPHONE  
		return ARNativePluginStatic.arwStartRunningB(vconf, cparaBuff, cparaBuffLen, nearPlane, farPlane);
        #else
		return ARNativePlugin.arwStartRunningB(vconf, cparaBuff, cparaBuffLen, nearPlane, farPlane);
        #endif
	}
	
	public static bool arwStartRunningStereoB(string vconfL, byte[] cparaBuffL, int cparaBuffLenL, string vconfR, byte[] cparaBuffR, int cparaBuffLenR, byte[] transL2RBuff, int transL2RBuffLen, float nearPlane, float farPlane)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwStartRunningStereoB(vconfL, cparaBuffL, cparaBuffLenL, vconfR, cparaBuffR, cparaBuffLenR, transL2RBuff, transL2RBuffLen, nearPlane, farPlane);
        #else
		return ARNativePlugin.arwStartRunningStereoB(vconfL, cparaBuffL, cparaBuffLenL, vconfR, cparaBuffR, cparaBuffLenR, transL2RBuff, transL2RBuffLen, nearPlane, farPlane);
        #endif 
	}

	public static bool arwIsRunning()
	{
		#if UNITY_IPHONE   
        return ARNativePluginStatic.arwIsRunning();
        #else
		return ARNativePlugin.arwIsRunning();
        #endif
	}

	public static bool arwStopRunning()
	{
		#if UNITY_IPHONE   
        return ARNativePluginStatic.arwStopRunning();
        #else		
        return ARNativePlugin.arwStopRunning();
        #endif
	}

	public static bool arwGetProjectionMatrix(float[] matrix)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetProjectionMatrix(matrix);
        #else 
		return ARNativePlugin.arwGetProjectionMatrix(matrix);
        #endif
	}

	public static bool arwGetProjectionMatrixStereo(float[] matrixL, float[] matrixR)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetProjectionMatrixStereo(matrixL, matrixR);
        #else 
		return ARNativePlugin.arwGetProjectionMatrixStereo(matrixL, matrixR);
        #endif
	}

	public static bool arwGetVideoParams(out int width, out int height, out int pixelSize, out String pixelFormatString)
	{
		StringBuilder sb = new StringBuilder(128);
		bool ok;
		#if UNITY_IPHONE  
        ok = ARNativePluginStatic.arwGetVideoParams(out width, out height, out pixelSize, sb, sb.Capacity);
        #else 
		ok = ARNativePlugin.arwGetVideoParams(out width, out height, out pixelSize, sb, sb.Capacity);
        #endif
		if (!ok) pixelFormatString = "";
		else pixelFormatString = sb.ToString();
		return ok;
	}

	public static bool arwGetVideoParamsStereo(out int widthL, out int heightL, out int pixelSizeL, out String pixelFormatL, out int widthR, out int heightR, out int pixelSizeR, out String pixelFormatR)
	{
		StringBuilder sbL = new StringBuilder(128);
		StringBuilder sbR = new StringBuilder(128);
		bool ok;
		#if UNITY_IPHONE   
        ok = ARNativePluginStatic.arwGetVideoParamsStereo(out widthL, out heightL, out pixelSizeL, sbL, sbL.Capacity, out widthR, out heightR, out pixelSizeR, sbR, sbR.Capacity);
        #else
		ok = ARNativePlugin.arwGetVideoParamsStereo(out widthL, out heightL, out pixelSizeL, sbL, sbL.Capacity, out widthR, out heightR, out pixelSizeR, sbR, sbR.Capacity);
        #endif
		if (!ok) {
			pixelFormatL = "";
			pixelFormatR = "";
		} else {
			pixelFormatL = sbL.ToString();
			pixelFormatR = sbR.ToString();
		}
		return ok;
	}

	public static bool arwCapture()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwCapture();
        #else
		return ARNativePlugin.arwCapture();
        #endif
	}

	public static bool arwUpdateAR()
	{
		#if UNITY_IPHONE   
        return ARNativePluginStatic.arwUpdateAR();
        #else
		return ARNativePlugin.arwUpdateAR();
        #endif
	}
	
    public static bool arwUpdateTexture([In, Out]Color[] colors)
	{
		bool ok;
		GCHandle handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
		IntPtr address = handle.AddrOfPinnedObject();
		#if UNITY_IPHONE   
        ok = ARNativePluginStatic.arwUpdateTexture(address);
        #else
		ok = ARNativePlugin.arwUpdateTexture(address);
        #endif
		handle.Free();
		return ok;
	}

	public static bool arwUpdateTextureStereo([In, Out]Color[] colorsL, [In, Out]Color[] colorsR)
	{
		bool ok;
		GCHandle handle0 = GCHandle.Alloc(colorsL, GCHandleType.Pinned);
		GCHandle handle1 = GCHandle.Alloc(colorsR, GCHandleType.Pinned);
		IntPtr address0 = handle0.AddrOfPinnedObject();
		IntPtr address1 = handle1.AddrOfPinnedObject();
		#if UNITY_IPHONE  
        ok = ARNativePluginStatic.arwUpdateTextureStereo(address0, address1);
        #else 
		ok = ARNativePlugin.arwUpdateTextureStereo(address0, address1);
        #endif
		handle0.Free();
		handle1.Free();
		return ok;
	}
	
	public static bool arwUpdateTexture32([In, Out]Color32[] colors32)
	{
		bool ok;
		GCHandle handle = GCHandle.Alloc(colors32, GCHandleType.Pinned);
		IntPtr address = handle.AddrOfPinnedObject();
		#if UNITY_IPHONE  
        ok = ARNativePluginStatic.arwUpdateTexture32(address);
        #else
		ok = ARNativePlugin.arwUpdateTexture32(address);
        #endif
		handle.Free();
		return ok;
	}
	
	public static bool arwUpdateTexture32Stereo([In, Out]Color32[] colors32L, [In, Out]Color32[] colors32R)
	{
		bool ok;
		GCHandle handle0 = GCHandle.Alloc(colors32L, GCHandleType.Pinned);
		GCHandle handle1 = GCHandle.Alloc(colors32R, GCHandleType.Pinned);
		IntPtr address0 = handle0.AddrOfPinnedObject();
		IntPtr address1 = handle1.AddrOfPinnedObject();
		#if UNITY_IPHONE  
        ok = ARNativePluginStatic.arwUpdateTexture32Stereo(address0, address1);
        #else 
		ok = ARNativePlugin.arwUpdateTexture32Stereo(address0, address1);
        #endif
		handle0.Free();
		handle1.Free();
		return ok;
	}
	
	public static bool arwUpdateTextureGL(int textureID)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwUpdateTextureGL(textureID);
        #else 
		return ARNativePlugin.arwUpdateTextureGL(textureID);
        #endif
	}
	
	public static bool arwUpdateTextureGLStereo(int textureID_L, int textureID_R)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwUpdateTextureGLStereo(textureID_L, textureID_R);
        #else
		return ARNativePlugin.arwUpdateTextureGLStereo(textureID_L, textureID_R);
        #endif
	}

	public static void arwSetUnityRenderEventUpdateTextureGLTextureID(int textureID)
	{
		#if UNITY_IPHONE   
        ARNativePluginStatic.arwSetUnityRenderEventUpdateTextureGLTextureID(textureID);
        #else
		ARNativePlugin.arwSetUnityRenderEventUpdateTextureGLTextureID(textureID);
        #endif
	}

	public static void arwSetUnityRenderEventUpdateTextureGLStereoTextureIDs(int textureID_L, int textureID_R)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetUnityRenderEventUpdateTextureGLStereoTextureIDs(textureID_L, textureID_R);
        #else
		ARNativePlugin.arwSetUnityRenderEventUpdateTextureGLStereoTextureIDs(textureID_L, textureID_R);
        #endif 
	}
	
	public static int arwGetMarkerPatternCount(int markerID)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetMarkerPatternCount(markerID);
        #else 
		return ARNativePlugin.arwGetMarkerPatternCount(markerID);
        #endif
	}

	public static bool arwGetMarkerPatternConfig(int markerID, int patternID, float[] matrix, out float width, out float height, out int imageSizeX, out int imageSizeY)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetMarkerPatternConfig(markerID, patternID, matrix, out width, out height, out imageSizeX, out imageSizeY);
        #else 
		return ARNativePlugin.arwGetMarkerPatternConfig(markerID, patternID, matrix, out width, out height, out imageSizeX, out imageSizeY);
        #endif
	}
	
	public static bool arwGetMarkerPatternImage(int markerID, int patternID, [In, Out]Color[] colors)
	{
		bool ok;
		#if UNITY_IPHONE  
        ok = ARNativePluginStatic.arwGetMarkerPatternImage(markerID, patternID, colors);
        #else
		ok = ARNativePlugin.arwGetMarkerPatternImage(markerID, patternID, colors);
        #endif
		return ok;
	}
	
	public static bool arwGetMarkerOptionBool(int markerID, int option)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetMarkerOptionBool(markerID, option);
        #else 
		return ARNativePlugin.arwGetMarkerOptionBool(markerID, option);
        #endif
	}
	
	public static void arwSetMarkerOptionBool(int markerID, int option, bool value)
	{
		#if UNITY_IPHONE 
        ARNativePluginStatic.arwSetMarkerOptionBool(markerID, option, value);
        #else 
		ARNativePlugin.arwSetMarkerOptionBool(markerID, option, value);
        #endif 
	}

	public static int arwGetMarkerOptionInt(int markerID, int option)
	{
		#if UNITY_IPHONE 
        return ARNativePluginStatic.arwGetMarkerOptionInt(markerID, option);
        #else 
		return ARNativePlugin.arwGetMarkerOptionInt(markerID, option);
        #endif
	}
	
	public static void arwSetMarkerOptionInt(int markerID, int option, int value)
	{
		#if UNITY_IPHONE 
        ARNativePluginStatic.arwSetMarkerOptionInt(markerID, option, value);
		#else
        ARNativePlugin.arwSetMarkerOptionInt(markerID, option, value);
        #endif
	}

	public static float arwGetMarkerOptionFloat(int markerID, int option)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetMarkerOptionFloat(markerID, option);
        #else
		return ARNativePlugin.arwGetMarkerOptionFloat(markerID, option);
        #endif 
	}
	
	public static void arwSetMarkerOptionFloat(int markerID, int option, float value)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetMarkerOptionFloat(markerID, option, value);
        #else 
		ARNativePlugin.arwSetMarkerOptionFloat(markerID, option, value);
        #endif
	}

	public static void arwSetVideoDebugMode(bool debug)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetVideoDebugMode(debug);
        #else
		ARNativePlugin.arwSetVideoDebugMode(debug);
        #endif
	}

	public static bool arwGetVideoDebugMode()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetVideoDebugMode();
        #else
		return ARNativePlugin.arwGetVideoDebugMode();
        #endif
	}

	public static void arwSetVideoThreshold(int threshold)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetVideoThreshold(threshold);
        #else
		ARNativePlugin.arwSetVideoThreshold(threshold); 
        #endif
	}

	public static int arwGetVideoThreshold()
	{
		#if UNITY_IPHONE 
        return ARNativePluginStatic.arwGetVideoThreshold();
        #else 
		return ARNativePlugin.arwGetVideoThreshold();
        #endif
	}

	public static void arwSetVideoThresholdMode(int mode)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetVideoThresholdMode(mode);
        #else 
		ARNativePlugin.arwSetVideoThresholdMode(mode);
        #endif
	}

	public static int arwGetVideoThresholdMode()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetVideoThresholdMode();
        #else 
		return ARNativePlugin.arwGetVideoThresholdMode();
        #endif
	}

	public static void arwSetLabelingMode(int mode)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetLabelingMode(mode);
        #else 
		ARNativePlugin.arwSetLabelingMode(mode);
        #endif
	}

	public static int arwGetLabelingMode()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetLabelingMode();
        #else 
		return ARNativePlugin.arwGetLabelingMode();
        #endif
	}

	public static void arwSetBorderSize(float size)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetBorderSize(size);
        #else 
		ARNativePlugin.arwSetBorderSize(size);
        #endif
	}

	public static float arwGetBorderSize()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetBorderSize();
        #else 
		return ARNativePlugin.arwGetBorderSize();
        #endif
	}

	public static void arwSetPatternDetectionMode(int mode)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetPatternDetectionMode(mode);
		#else 
        ARNativePlugin.arwSetPatternDetectionMode(mode);
        #endif
	}

	public static int arwGetPatternDetectionMode()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetPatternDetectionMode();
        #else 
		return ARNativePlugin.arwGetPatternDetectionMode();
        #endif
	}

	public static void arwSetMatrixCodeType(int type)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetMatrixCodeType(type);
        #else 
		ARNativePlugin.arwSetMatrixCodeType(type);
        #endif
	}

	public static int arwGetMatrixCodeType()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetMatrixCodeType();
        #else
		return ARNativePlugin.arwGetMatrixCodeType();
        #endif
	}

	public static void arwSetImageProcMode(int mode)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetImageProcMode(mode);
        #else 
		ARNativePlugin.arwSetImageProcMode(mode);
        #endif
	}

	public static int arwGetImageProcMode()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetImageProcMode();
        #else 
		return ARNativePlugin.arwGetImageProcMode();
        #endif
	}
	
	public static void arwSetNFTMultiMode(bool on)
	{
		#if UNITY_IPHONE  
        ARNativePluginStatic.arwSetNFTMultiMode(on);
        #else 
		ARNativePlugin.arwSetNFTMultiMode(on);
        #endif
	}

	public static bool arwGetNFTMultiMode()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwGetNFTMultiMode();
        #else 
		return ARNativePlugin.arwGetNFTMultiMode();
        #endif
	}

	public static int arwAddMarker(string cfg)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwAddMarker(cfg);
        #else 
		return ARNativePlugin.arwAddMarker(cfg);
        #endif
	}
	
	public static bool arwRemoveMarker(int markerID)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwRemoveMarker(markerID);
        #else 
		return ARNativePlugin.arwRemoveMarker(markerID);
        #endif
	}

	public static int arwRemoveAllMarkers()
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwRemoveAllMarkers();
        #else 
		return ARNativePlugin.arwRemoveAllMarkers();
        #endif
	}

	public static bool arwQueryMarkerVisibility(int markerID)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwQueryMarkerVisibility(markerID);
        #else 
		return ARNativePlugin.arwQueryMarkerVisibility(markerID);
        #endif
	}

	public static bool arwQueryMarkerTransformation(int markerID, float[] matrix)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwQueryMarkerTransformation(markerID, matrix);
        #else 
		return ARNativePlugin.arwQueryMarkerTransformation(markerID, matrix);
        #endif
	}

	public static bool arwQueryMarkerTransformationStereo(int markerID, float[] matrixL, float[] matrixR)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwQueryMarkerTransformationStereo(markerID, matrixL, matrixR);
        #else 
		return ARNativePlugin.arwQueryMarkerTransformationStereo(markerID, matrixL, matrixR);
        #endif
	}
	
	public static bool arwLoadOpticalParams(string optical_param_name, byte[] optical_param_buff, int optical_param_buffLen, out float fovy_p, out float aspect_p, float[] m, float[] p)
	{
		#if UNITY_IPHONE  
        return ARNativePluginStatic.arwLoadOpticalParams(optical_param_name, optical_param_buff, optical_param_buffLen, out fovy_p, out aspect_p, m, p);
        #else 
		return ARNativePlugin.arwLoadOpticalParams(optical_param_name, optical_param_buff, optical_param_buffLen, out fovy_p, out aspect_p, m, p);
        #endif
    }

}
