import cv2
import imutils
import numpy as np

def main():
    image = cv2.imread('C:\\Users\\alaa salah younes\Desktop\\pic.png')
    resized = imutils.resize(image, width=400)
    ratio = image.shape[0] / float(resized.shape[0])

    # convert the resized image to grayscale, blur it slightly,
    # and threshold it
    gray = cv2.cvtColor(resized, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    thresh = cv2.threshold(blurred, 100, 255, cv2.THRESH_BINARY)[1]
    cv2.imshow("orginalimage", thresh)

    median = cv2.medianBlur(thresh, 5)
    end = np.invert(median)
    cv2.imshow("Inverted", end)
    # find contours in the thresholded image and initialize the
    # shape detector
    cnts = cv2.findContours(end.copy(), cv2.RETR_EXTERNAL,
                            cv2.CHAIN_APPROX_SIMPLE)
    cnts = cnts[0] if imutils.is_cv2() else cnts[1]
    tri_c=0
    cir_c=0
    squ_c=0
    lin_c=0

    # loop over the contours
    for c in cnts:
        # compute the center of the contour, then detect the name of the
        # shape using only the contour
        M = cv2.moments(c)
        cX = int((M["m10"] / M["m00"]) * ratio)
        cY = int((M["m01"] / M["m00"]) * ratio)
        shape,tri_c,cir_c,squ_c,lin_c = detect(c,tri_c,cir_c,squ_c,lin_c)

        # multiply the contour (x, y)-coordinates by the resize ratio,
        # then draw the contours and the name of the shape on the image
        c = c.astype("float")
        c *= ratio
        c = c.astype("int")
        cv2.drawContours(image, [c], -1, (0, 255, 0), 2)
        cv2.putText(image, shape, (cX, cY), cv2.FONT_HERSHEY_SIMPLEX,0.5, (0, 255, 255), 2)

        # show the output image
        cv2.imshow("Image", image)
        #cv2.waitKey(0)

    print("%d triangles"%tri_c ,"%d circuls"%cir_c,"%d squerss"%squ_c,"%d lines"%lin_c )
    output(tri_c,cir_c,squ_c,lin_c)


    return
# end main

##################################################################################################
def output(tri_c,cir_c,squ_c,lin_c):

    font = cv2.FONT_HERSHEY_SIMPLEX
    img = np.zeros((512, 512, 3), np.uint8)
    ################################################################

    cv2.putText(img, "%d"%cir_c, (10, 30), font, 0.8, (0, 0, 255), 2, cv2.LINE_AA)
    img = cv2.circle(img, (130, 30), 30, (0, 0, 255), -1)
    ################################################################
    cv2.putText(img,"%d"% tri_c, (10, 120), font, 0.8, (0,0, 255), 2, cv2.LINE_AA)
    points = np.array(([160, 140], [130, 90], [100, 140]))
    points = points.reshape((-1, 1, 2))
    img = cv2.polylines(img, [points], True, (0, 0, 255), 35)
    ################################################################

    cv2.putText(img, "%d"%lin_c, (10, 220), font, 0.8, (0, 0, 255), 2, cv2.LINE_AA)
    img = cv2.line(img, (130, 200), (130, 250), (0, 0, 255), 3)
    ################################################################
    cv2.putText(img, "%d"%squ_c, (10, 330), font, 0.8, (0,0, 255), 2, cv2.LINE_AA)
    img = cv2.rectangle(img, (110, 350), (160, 300), (0, 0, 255), -1)
    ################################################################

    cv2.imshow('Draw01', img)
    cv2.waitKey(0)
    return 0
###################################################################################################

def detect(c,tri_c,cir_c,squ_c,lin_c):
    # initialize the shape name and approximate the contour
    shape = "unidentified"
    peri = cv2.arcLength(c, True)
    approx = cv2.approxPolyDP(c, 0.04 * peri, True)

    # if the shape is a triangle, it will have 3 vertices
    if len(approx) == 2:
        shape = "line"
        lin_c += 1

    elif len(approx) == 3:
        shape = "triangle"
        tri_c += 1

    # if the shape has 4 vertices, it is either a square or
    # a rectangle
    elif len(approx) == 4:
        # compute the bounding box of the contour and use the
        # bounding box to compute the aspect ratio
        (x, y, w, h) = cv2.boundingRect(approx)
        ar = w / float(h)
        if (ar >= 0.95 and ar <= 1.05):
            shape = "square"
            squ_c+=1
        else:
            shape = "line"
            lin_c+=1

    # a square will have an aspect ratio that is approximately
    # equal to one, otherwise, the shape is a rectangle

    # otherwise, we assume the shape is a circle
    else:
        shape = "circle"
        cir_c+=1


    # return the name of the shape
    return shape,tri_c,cir_c,squ_c,lin_c
# end function